﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

namespace ResourceManager.Helpers
{
    public class Filter<T> where T : class
    {
        public IEnumerable<T> FilteredData(FilterParams filterParam, IEnumerable<T> data)
        {
            var filterColumn = typeof(T).GetProperty(filterParam.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (filterColumn != null)
            {
                return FilterData(filterParam.FilterOption, data, filterColumn, filterParam.FilterValue);
            }

            return Enumerable.Empty<T>();
        }
        private IEnumerable<T> FilterData(FilterOptions filterOption, IEnumerable<T> data, PropertyInfo filterColumn, string filterValue)
        {
            int outValue;
            DateTime dateValue;
            switch (filterOption)
            {
                #region [StringDataType]  

                case FilterOptions.StartsWith:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().StartsWith(filterValue.ToString().ToLower())).ToList();
                    break;
                case FilterOptions.EndsWith:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().EndsWith(filterValue.ToString().ToLower())).ToList();
                    break;
                case FilterOptions.Contains:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().Contains(filterValue.ToString().ToLower())).ToList();
                    break;
                case FilterOptions.DoesNotContain:
                    data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                     (filterColumn.GetValue(x, null) != null && !filterColumn.GetValue(x, null).ToString().ToLower().Contains(filterValue.ToString().ToLower()))).ToList();
                    break;
                case FilterOptions.IsEmpty:
                    data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                     (filterColumn.GetValue(x, null) != null && string.IsNullOrWhiteSpace(filterColumn.GetValue(x, null)?.ToString()))).ToList();
                    break;
                case FilterOptions.IsNotEmpty:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && !string.IsNullOrWhiteSpace(filterColumn.GetValue(x, null)?.ToString())).ToList();
                    break;
                #endregion

                #region [Custom]  

                case FilterOptions.IsGreaterThan:
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) > outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) > dateValue).ToList();

                    }
                    break;

                case FilterOptions.IsGreaterThanOrEqualTo:
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) >= outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) >= dateValue).ToList();
                        break;
                    }
                    break;

                case FilterOptions.IsLessThan:
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) < outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) < dateValue).ToList();
                        break;
                    }
                    break;

                case FilterOptions.IsLessThanOrEqualTo:
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) <= outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) <= dateValue).ToList();
                        break;
                    }
                    break;

                case FilterOptions.IsEqualTo:
                    if (filterValue == string.Empty)
                    {
                        data = data.Where(x => filterColumn.GetValue(x, null) == null
                                        || (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() == string.Empty)).ToList();
                    }
                    else
                    {
                        if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                        {
                            data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) == outValue).ToList();
                        }
                        else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                        {
                            data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) == dateValue).ToList();
                            break;
                        }
                        else
                        {
                            data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() == filterValue.ToLower()).ToList();
                        }
                    }
                    break;

                case FilterOptions.IsNotEqualTo:
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) != outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) != dateValue).ToList();
                        break;
                    }
                    else
                    {
                        data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                         (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() != filterValue.ToLower())).ToList();
                    }
                    break;
                    #endregion
            }
            return data;
        }

    }
}