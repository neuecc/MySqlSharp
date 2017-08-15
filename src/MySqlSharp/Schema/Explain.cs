using MySqlSharp.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Schema
{
    // https://dev.mysql.com/doc/refman/5.6/en/explain-output.html

    public class Explain
    {

    }


    public class StringToExplainSelectTypeConverter : IValueConverter<string, ExplainSelectType>
    {
        public ExplainSelectType Convert(string value)
        {
            switch (value)
            {
                case nameof(ExplainSelectType.SIMPLE):
                    return ExplainSelectType.SIMPLE;
                case nameof(ExplainSelectType.PRIMARY):
                    return ExplainSelectType.PRIMARY;
                case nameof(ExplainSelectType.UNION):
                    return ExplainSelectType.UNION;
                case "DEPENDENT UNION":
                    return ExplainSelectType.DEPENDENT_UNION;
                case "UNION RESULT":
                    return ExplainSelectType.UNION_RESULT;
                case nameof(ExplainSelectType.SUBQUERY):
                    return ExplainSelectType.SUBQUERY;
                case "DEPENDENT SUBQUERY":
                    return ExplainSelectType.DEPENDENT_SUBQUERY;
                case nameof(ExplainSelectType.MATERIALIZED):
                    return ExplainSelectType.MATERIALIZED;
                case "UNCACHEABLE SUBQUERY":
                    return ExplainSelectType.UNCACHEABLE_SUBQUERY;
                case "UNCACHEABLE UNION":
                    return ExplainSelectType.UNCACHEABLE_UNION;
                default:
                    throw new Exception("Unknown ExplainSelectType");
            }
        }
    }

    public enum ExplainSelectType
    {
        ///<summary>Simple SELECT (not using UNION or subqueries)</summary>
        SIMPLE,
        ///<summary>Outermost SELECT</summary>
        PRIMARY,
        ///<summary>Second or later SELECT statement in a UNION</summary>
        UNION,
        ///<summary>Second or later SELECT statement in a UNION, dependent on outer query</summary>
        DEPENDENT_UNION,
        ///<summary>Result of a UNION.</summary>
        UNION_RESULT,
        ///<summary>First SELECT in subquery</summary>
        SUBQUERY,
        ///<summary>First SELECT in subquery, dependent on outer query</summary>
        DEPENDENT_SUBQUERY,
        ///<summary>Derived table SELECT (subquery in FROM clause)</summary>
        ///DERIVED,
        ///<summary>Materialized subquery</summary>
        MATERIALIZED,
        ///<summary>A subquery for which the result cannot be cached and must be re-evaluated for each row of the outer query</summary>
        UNCACHEABLE_SUBQUERY,
        ///<summary>The second or later select in a UNION that belongs to an uncacheable subquery (see UNCACHEABLE SUBQUERY)</summary>
        UNCACHEABLE_UNION
    }
}
