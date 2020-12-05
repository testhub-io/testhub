using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestHub.Api.ApiDataProvider
{
    public class IdGenerator
    {
        Dictionary<string, int> testsList = new Dictionary<string, int>();
        int idCounter = 0;

        public int GetId(string name)
        {
            if (testsList.ContainsKey(name))
            {
                return testsList[name];
            }
            else
            {
                idCounter++;
                testsList[name] = idCounter;
                return idCounter;
            }
        }



    }
}
