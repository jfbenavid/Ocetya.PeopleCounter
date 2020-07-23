namespace GranEstacion.Repository
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        public DateTime Date { get; set; }

        public int CameraId { get; set; }

        public Camera Camera { get; set; }

        public int Entered { get; set; }

        public int Exited { get; set; }
    }
}