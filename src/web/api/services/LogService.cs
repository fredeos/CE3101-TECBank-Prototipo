using System.Threading.Tasks;
using tecbank.services.logger;

namespace tecbank.services {
    public class LogService {
        // --------------------------------[ Service attributes ]--------------------------------
        private static LogConsole __console = new LogConsole("tecbank");
        private int use_count = 0;

        private static SemaphoreSlim __lock = new SemaphoreSlim(1);
        // --------------------------------[ Service functions and methods ]--------------------------------
        public void Log_New(LogTypes type, String msg){
            try{
                __lock.Wait();
                if (this.use_count == 0) {
                    __console.bootup(1000);
                    use_count+=1;
                }
                __console.log(type, msg);
            } finally{
                __lock.Release();
            }
        }
    }
}