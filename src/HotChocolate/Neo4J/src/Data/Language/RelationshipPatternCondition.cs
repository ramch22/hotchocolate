using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotChocolate.Data.Neo4J.Language.Clauses
{
    class RelationshipPatternCondition : Condition
    {
        public override ClauseKind Kind => ClauseKind.Default;
    }
}