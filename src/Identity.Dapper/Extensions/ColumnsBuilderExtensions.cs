﻿using Identity.Dapper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Identity.Dapper
{
    public static class ColumnsBuilderExtensions
    {
        public static string GetCommaSeparatedColumns(this IEnumerable<string> properties)
        {
            var columnsBuilder = new StringBuilder();

            var valuesArray = new List<string>(properties.Count());

            columnsBuilder.Append(string.Join(", ", properties));

            return columnsBuilder.ToString();
        }

        public static IEnumerable<string> GetColumns<TEntity>(this TEntity entity, SqlConfiguration sqlConfiguration, bool ignoreIdProperty = false, IEnumerable<string> ignoreProperties = null, bool forInsert = true)
        {
            ignoreProperties = ignoreProperties ?? Enumerable.Empty<string>();

            var roleProperties = Enumerable.Empty<string>();
            var idProperty = entity.GetType().GetProperty("Id");

            if (idProperty != null && !ignoreIdProperty)
            {
                var defaultIdTypeValue = idProperty.PropertyType == typeof(string) ? string.Empty : Activator.CreateInstance(idProperty.PropertyType);
                var idPropertyValue = idProperty.GetValue(entity, null);

                roleProperties = !idPropertyValue.Equals(defaultIdTypeValue)
                                    ? entity.GetType()
                                            .GetPublicPropertiesNames(x => !ignoreProperties.Any(y => x.Name == y) 
                                                                        && !x.CustomAttributes.Any(y => y.AttributeType == typeof(NotMappedAttribute)))
                                    : forInsert 
                                        ? entity.GetType()
                                                .GetPublicPropertiesNames(y => !y.Name.Equals("Id")
                                                                               && !ignoreProperties.Any(x => x == y.Name)
                                                                               && !y.CustomAttributes.Any(x => x.AttributeType == typeof(NotMappedAttribute)))
                                        : entity.GetType()
                                                .GetPublicPropertiesNames(x => !ignoreProperties.Any(y => x.Name == y));
            }
            else
            {
                roleProperties = entity.GetType()
                                       .GetPublicPropertiesNames(y => !y.Name.Equals("Id")
                                                                      && !ignoreProperties.Any(x => x == y.Name)
                                                                      && !y.CustomAttributes.Any(x => x.AttributeType == typeof(NotMappedAttribute)));
            }

            roleProperties = roleProperties.Select(y => string.Concat(sqlConfiguration.TableColumnStartNotation, y, sqlConfiguration.TableColumnEndNotation));

            return roleProperties;
        }
    }
}
