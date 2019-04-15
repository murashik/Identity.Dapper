using Identity.Dapper.Models;
using Identity.Dapper.Queries.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Dapper.Queries.User
{
    public class GetTopUsers : ISelectQuery
    {
        private readonly SqlConfiguration _sqlConfiguration;
        public GetTopUsers(SqlConfiguration sqlConfiguration)
        {
            _sqlConfiguration = sqlConfiguration;
        }

        public string GetQuery()
        {
            var query = _sqlConfiguration.SelectUserByIdQuery
                                         .ReplaceQueryParameters(_sqlConfiguration.SchemaName,
                                                                 string.Empty,
                                                                 _sqlConfiguration.ParameterNotation,
                                                                new string[] {
                                                                                "%TOPROWS%",
                                                                                "%LIMIT%"
                                                                              },
                                                                 new string[] {
                                                                                "TopRows",
                                                                                "Limit"
                                                                              },
                                                                 new string[] {
                                                                                "%USERTABLE%"
                                                                              },
                                                                 new string[] {
                                                                                _sqlConfiguration.UserTable,
                                                                              }
                                                                 );

            return query;
        }

        public string GetQuery<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
