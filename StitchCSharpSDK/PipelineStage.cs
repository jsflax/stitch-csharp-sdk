using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stitch
{
    // A PipelineStage represents a single stage within a pipeline that specifies
    // an action, service, and its arguments.
    public struct PipelineStage
    {
        // The action that represents this stage.
        [JsonProperty(PropertyName = "action", 
                      NullValueHandling = NullValueHandling.Ignore)]
        private readonly string _action;

        /**
         * The service that can handle the {@link PipelineStage#_action}. A null
         * service means that the {@link PipelineStage#_action} is builtin.
         */
        [JsonProperty(PropertyName = "service",
                      NullValueHandling = NullValueHandling.Ignore)]
        private readonly string _service;

        /**
		 * The arguments to invoke the action with.
		 */
        [JsonProperty(PropertyName = "args",
                      NullValueHandling = NullValueHandling.Ignore)]
        private readonly Dictionary<string, object> _args;

        /**
		 * The expression to evaluate for use within the arguments via expansion.
		 */
        [JsonProperty(PropertyName = "let",
                      NullValueHandling = NullValueHandling.Ignore)]
        private readonly object _let;

        /**
         * Constructs a completely specified pipeline stage
         *
         * @param action  The action that represents this stage.
         * @param service The service that can handle the {@code action}. A null
         *                service means that the {@code action} is builtin.
         * @param args    The arguments to invoke the action with.
         * @param let     The expression to evaluate for use within the arguments via expansion.
         */
        public PipelineStage(string action,
                             string service = null,
                             Dictionary<string, object> args = null,
                             object let = null)
        {
            _action = action;
            _service = service;
            _args = args;
            _let = let;
        }

        public static class LiteralStage
        {
            public const string NAME = "literal";
            public const string PARAMETER_ITEMS = "items";
        }
    }

}
