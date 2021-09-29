using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Shared;

namespace Inspections.Core.Domain
{
    // id, circuit_id, channel, date, current_10cycle, current_1cycle,
    //     current_l1, current_l2, current_l3 
    [Table("current")]
    public class Current
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("circuit_id")]
        public int CircuitId { get; set; }
        public string Channel { get; set; }
        public DateTime Date { get; set; }
        [Column("current_10cycle")]
        public double Current10Cycle { get; set; }
        [Column("current_1cycle")]
        public double Current1Cycle { get; set; }
        [Column("current_l1")]
        public double CurrentL1 { get; set; }
        [Column("current_l2")]
        public double CurrentL2 { get; set; }
        [Column("current_l3")]
        public double CurrentL3 { get; set; }

        public override string ToString()
        {
            return $"{Id} {CircuitId} {Channel} {Date} {CurrentL1} {CurrentL2} {CurrentL3}";
        }
    }
}
