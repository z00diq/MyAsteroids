using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Scripts.Remotes
{
    public interface IRemoteConfig
    {
        public Task FetchDataAsync();

        public Task ActivateDataAsync(Task previous);

        public Task LoadDataAsync(Task previous);
    }
}
