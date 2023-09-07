using LittleKingdom;
using LittleKingdom.Resources;
using System.Collections.Generic;

namespace ResourceRequestsTests
{
    public record TestRequest(IHoldResources From, IHoldResources To);

    public interface IResourceRequestsTestsHandler : IHandleResources<TestRequest>
    {

    }

    public class ResourceRequestsTestsHandler : IResourceRequestsTestsHandler
    {
        public IPlayer Player { get; set; }
        public IEnumerable<TestRequest> Requests { get; set; }

        public IEnumerable<TestRequest> GetRequests() => Requests;
    }

    public class TestHandlerOne : ResourceRequestsTestsHandler
    {

    }

    public class TestHandlerTwo : ResourceRequestsTestsHandler
    {

    }
}