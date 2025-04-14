namespace tecbank.services {
    public class PasswordService {
        public int AdminPassKey {get;}

        public PasswordService(){
            this.AdminPassKey = new Random().Next(100000,999999);
        }
    }
}