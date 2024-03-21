using UnityEngine;

namespace _Main.Scripts.Steering_Behaviours
{
    public static class MySteeringBehaviors
    {
        /// <returns>
        ///   <para>Devuelve una direccion directa para INTERCEPTAR  al objetivo X SEGUNDOS adelante en su trayectoria.</para>
        /// </returns>
        public static Vector2 GetInterceptDir(Vector2 p_origin, Vector2 p_targetPos, Vector2 p_targetLookDir,float p_targetSpeed, float p_time)
        {
            Vector2 l_point = p_targetPos + (p_targetLookDir * Mathf.Clamp(p_targetSpeed * p_time, 0, 100));
            return (l_point - p_origin).normalized;
        }
        /// <returns>
        ///   <para>Devuelve una direccion directa para HUIR  del la futura posicion del objetivo X SEGUNDOS adelante.</para>
        /// </returns>
        public static Vector2 GetEvadeDir(Vector2 p_originPos, Vector2 p_targetPos, Vector2 p_targetLookDir,float p_targetSpeed, float p_time)
        {
            Vector2 l_point = p_targetPos + (p_targetLookDir * Mathf.Clamp(p_targetSpeed * p_time, 0, 100));
            return -(l_point - p_originPos).normalized;
        }
        /// <returns>
        ///   <para>Devuelve una direccion directa para ir HUIR el objetivo.</para>
        /// </returns>
        public static Vector2 GetFleeDir(Vector2 p_originPos, Vector2 p_targetPos)
        {
            return -(p_targetPos -p_originPos).normalized;
        }
        
        /// <returns>
        ///   <para>Devuelve una direccion directa para IR HACIA el objetivo.</para>
        /// </returns>
        public static Vector3 GetChaseDir(Vector2 p_originPos, Vector2 p_targetPos)
        {
            return (p_targetPos - p_originPos).normalized;
        }
        
        
        /// <summary>
        ///   <para> Obstacle Avoidace</para>
        /// </summary>
        /// <param name="p_detectionRaduis"> El radio de deteccion para obstaculos.</param>
        /// <param name="p_viewAngle"> Es el angulo de vision al que la entidad presta atencion para ver obstaculos.</param>
        /// <returns>
        ///   <para>Devuelve una direccion optima para esquivar todos los obstaculos dentro del rango.</para>
        /// </returns>
        public static Vector3 GetObsAvoidanceDir(Vector2 p_orginPos, Vector2 p_originLookDir ,float p_detectionRaduis, float p_viewAngle,LayerMask l_mask)
        {
            var l_allObs = new Collider2D[10];
            var l_countObs = Physics2D.OverlapCircleNonAlloc(p_orginPos, p_detectionRaduis, l_allObs, l_mask);
            Vector2 l_dirToAvoid = Vector2.zero;
            int l_trueObs = 0;
            for (int l_i = 0; l_i < l_countObs; l_i++)
            {
                var l_currObs = l_allObs[l_i];
                var l_closestPoint = l_currObs.ClosestPoint(p_orginPos);
                var l_diffToPoint = (Vector2)l_closestPoint - p_orginPos;
                
                var l_angleToPoint = Vector3.Angle(p_originLookDir, l_diffToPoint.normalized);
                
                if(l_angleToPoint > p_viewAngle/2)
                    continue;
                
                float l_dist = l_diffToPoint.magnitude;
                
                l_trueObs++;
                l_dirToAvoid += -(l_diffToPoint).normalized * (p_detectionRaduis - l_dist);

            }
            
            if(l_trueObs != 0)
                l_dirToAvoid /= l_trueObs;

            return l_dirToAvoid;
        }
        
        public static Vector3 GetAdvancedObsAvoidanceDir(Vector2 p_orginPos,Vector2 p_targetPosition,float p_detectionRaduis, float p_behabiourIntensity,LayerMask l_mask)
        {
            var l_checkRadius = p_detectionRaduis;
            var l_selfPosition = p_orginPos;
            var l_dir = (p_targetPosition - l_selfPosition);
            var m_obstaclesColliders = new Collider2D[20];

            var l_countObjs = Physics2D.OverlapCircleNonAlloc(l_selfPosition, l_checkRadius, m_obstaclesColliders, l_mask);

            Collider2D l_nearestObject = default;
            float l_distanceNearObj = 0;

            for (var l_i = 0; l_i < l_countObjs; l_i++)
            {
                var l_curr = m_obstaclesColliders[l_i];

                if (l_selfPosition == (Vector2)l_curr.transform.position) 
                    continue;

                var l_closestPointToSelf = l_curr.ClosestPoint(l_selfPosition);
                var l_distanceCurr = Vector2.Distance(l_selfPosition, l_closestPointToSelf);

                if (l_nearestObject == default)
                {
                    l_nearestObject = l_curr;
                    l_distanceNearObj = l_distanceCurr;
                }
                else
                {
                    var l_distance = Vector2.Distance(l_selfPosition, l_curr.transform.position);

                    if (!(l_distanceNearObj > l_distance))
                        continue;

                    l_nearestObject = l_curr;
                    l_distanceNearObj = l_distanceCurr;
                }
            }

            if (l_nearestObject == default)
                return l_dir.normalized;

            Vector2 l_posObj = l_nearestObject.transform.position;
            var l_dirObstacleToSelf = l_selfPosition - l_posObj;

            l_dirObstacleToSelf = l_dirObstacleToSelf.normalized * (((l_checkRadius - l_distanceNearObj) / l_checkRadius) * p_behabiourIntensity);

            l_dir += l_dirObstacleToSelf;

            return l_dir.normalized;
        }
        
    }
}
