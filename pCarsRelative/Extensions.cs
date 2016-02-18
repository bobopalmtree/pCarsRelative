using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pCarsRelative
{
    public static class Extensions
    {
        public static void Shift<T>(this List<T> list, int places)
        {
            try
            {
                if (places > 0)
                {
                    for (int i = places; i > 0; i--)
                    {
                        T temp = list[0];
                        list.RemoveAt(0);
                        list.Add(temp);
                    }
                }
                else
                {
                    for (int i = places; i < 0; i++)
                    {
                        T temp = list[list.Count - 1];
                        list.RemoveAt(list.Count - 1);
                        list.Insert(0, temp);
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
