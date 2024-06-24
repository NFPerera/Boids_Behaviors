using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Main.Scripts.DevelopmentUtilities.Extensions
{
    public static class ListExtensions
    {
        public static T GetLastElement<T>(this List<T> p_baseList) => p_baseList[^1]; 
        
        public static T GetRandomElement<T>(this List<T> p_baseList)
        {
            return p_baseList[Random.Range(0, p_baseList.Count)];
        }
        
        public static List<T> GetUnmatchedElements<T>(ICollection<T> p_list1, List<T> p_list2)
        {
            List<T> l_unmatchedList = p_list1.Except(p_list2).ToList();
            l_unmatchedList.AddRange(p_list2.Except(p_list1));

            return l_unmatchedList;
        }
        
        public static void RemoveLast<T>(this List<T> p_list)
        {
            if (p_list == null || p_list.Count == 0)
            {
                return; // Alternatively, you might want to throw an exception if the list is null or empty.
            }

            p_list.RemoveAt(p_list.Count - 1);
        }
    }
}