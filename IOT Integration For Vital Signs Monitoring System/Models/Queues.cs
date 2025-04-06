using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IOT_Integration_For_Vital_Signs_Monitoring_System.Models
{
    public class Queues
    {
        //QUEUE: Data for listing or adding vital datas
        [Key] // This is the Primary key or ID
        public int Id { get; set; }

        public string QueueName { get; set; }

        public string QueueLastName { get; set; }

        public string Passcode { get; set; }
    }
}
