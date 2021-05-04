using AutoBuildApp.DataAccess.DataTransferObjects;
using System.Collections.Generic;
using AutoBuildApp.Managers;

namespace AutoBuildApp.Services
{
    public class SearchService
    {
        private SearchGateway _gateway;

        /*
        public SearchService(string connection) 
        {
            _gateway = new SearchGateway(connection);
        }*/

        public ISet<IResult> Search(string searchString, string resultType) {
            return _gateway.Search(searchString, resultType);
        }
    }
}
