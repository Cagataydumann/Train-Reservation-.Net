using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Tren_Rezervasyon.Helpers
{
    public class ConflictingActionsResolver : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var matchedActions = controller.Actions.GroupBy(x => x.ActionName).Where(x => x.Count() > 1).ToList();

                foreach (var matchedActionGroup in matchedActions)
                {
                    var methods = matchedActionGroup.Select(x => x.Selectors.FirstOrDefault()?.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods?.FirstOrDefault()).Distinct();

                    if (methods.Count() > 1)
                    {
                        var message = $"Conflicting HTTP methods detected for endpoint {matchedActionGroup.Key}.";

                        foreach (var matchedAction in matchedActionGroup)
                        {
                            message += $" {matchedAction.DisplayName} ({matchedAction.Selectors.FirstOrDefault()?.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods?.FirstOrDefault()})";
                        }

                        throw new InvalidOperationException(message);
                    }
                }
            }
        }
    }
}
