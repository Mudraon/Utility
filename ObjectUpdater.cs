using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class ObjectUpdater
{
    public static void SimpleInstanceUpdater<T>(T aSource, T aDestination)
    {
        if (aSource == null || aDestination == null)
            return;

        Type lDestinationType = typeof(T);
        PropertyInfo[] lProperties = lDestinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo lProperty in lProperties)
        {
            Type lPropertyType = lProperty.PropertyType;
            if (lPropertyType.IsArray || (lPropertyType.IsGenericType && lPropertyType.GetGenericTypeDefinition() == typeof(List<>)))
            {
                IList lDestinationList = (IList)lProperty.GetValue(aDestination);
                IList lSourceList = (IList)lProperty.GetValue(aSource);

                if (lDestinationList != null && lSourceList != null)
                {
                    foreach (var lSourceElement in lSourceList)
                    {
                        bool lElementExist = false;
                        int lElementIdx = -1;

                        for (int i = 0; i < lDestinationList.Count; i++)
                        {
                            var lDestinationElement = lDestinationList[i];
                            if (ObjectEqual(lDestinationElement, lSourceElement))
                            {
                                lElementIdx = i;
                                lElementExist = true;
                                break;
                            }
                        }

                        if (lElementExist)
                        {
                            if (lSourceElement.GetType().IsClass && lSourceElement.GetType() != typeof(string))
                                GenericMethod(lSourceElement, lDestinationList[lElementIdx]);
                        }
                        else
                        {
                            object lNewDestinationElement = CloneObject(lSourceElement);
                            if (lSourceElement.GetType().IsClass && lSourceElement.GetType() != typeof(string))
                                GenericMethod(lSourceElement, lNewDestinationElement);
                            lDestinationList.Add(lNewDestinationElement);
                        }
                    }
                }
            }
        }
    }

    private static void GenericMethod<T>(T aSource, T aDestination)
    {
        Type lElementType = aSource.GetType();
        MethodInfo lMethod = typeof(ObjectUpdater).GetMethod("SimpleInstanceUpdater");
        MethodInfo lGenericMethod = lMethod.MakeGenericMethod(lElementType);
        lGenericMethod.Invoke(null, new object[] { aSource, aDestination });
    }

    private static bool ObjectEqual(object aItem1, object aItem2)
    {
        if (aItem1 == null && aItem2 == null)
            return true;

        if (aItem1 == null || aItem2 == null)
            return false;

        if (aItem1.GetType() != aItem2.GetType())
            return false;

        if (aItem1.Equals(aItem2))
            return true;

        return false;
    }

    private static object CloneObject(object aItem)
    {
        if (aItem == null)
            return null;

        Type lType = aItem.GetType();
        MethodInfo lMemberwiseCloneMethod = lType.GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
        return lMemberwiseCloneMethod.Invoke(aItem, null);
    }
}