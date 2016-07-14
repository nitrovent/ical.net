using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Ical.Net.Serialization.iCalendar.Serializers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ical.NET.UnitTests
{
    [TestFixture]
    public class AttendeeTest
    {
        internal static Event VEventFactory()
        {
            return new Event
            {
                Summary = "Testing",
                Start = new CalDateTime(2010, 3, 25),
                End = new CalDateTime(2010, 3, 26)
            };
        }

        private const string _requiredParticipant = "REQ-PARTICIPANT"; //this string may be added to the api in the future
        private static readonly IList<Attendee> _attendees = new List<Attendee>
        {
            new Attendee("MAILTO:james@example.com")
            {
                CommonName = "James James",
                Role = _requiredParticipant,
                Rsvp = true,
                ParticipationStatus = ParticipationStatus.Tentative
            },
            new Attendee("MAILTO:mary@example.com")
            {
                CommonName = "Mary Mary",
                Role = _requiredParticipant,
                Rsvp = true,
                ParticipationStatus = ParticipationStatus.Accepted
            }
        }.AsReadOnly();

        
        /// <summary>
        /// Ensures that attendees can be properly added to an event.
        /// </summary>
        [Test, Category("Attendee")]
        public void Add1Attendee()
        {
            var evt = VEventFactory();
            Assert.AreEqual(0, evt.Attendees.Count);

            evt.Attendees.Add(_attendees[0]);
            Assert.AreEqual(1, evt.Attendees.Count);

            //the properties below had been set to null during the Attendees.Add operation in NuGet version 2.1.4
            Assert.AreEqual(_requiredParticipant, evt.Attendees[0].Role); 
            Assert.AreEqual(ParticipationStatus.Tentative, evt.Attendees[0].ParticipationStatus);
        }

        [Test, Category("Attendee")]
        public void Add2Attendees()
        {
            var evt = VEventFactory();
            Assert.AreEqual(0, evt.Attendees.Count);

            evt.Attendees.Add(_attendees[0]);
            evt.Attendees.Add(_attendees[1]);
            Assert.AreEqual(2, evt.Attendees.Count);
            Assert.AreEqual(_requiredParticipant, evt.Attendees[1].Role);

            var cal = new Calendar();
            cal.Events.Add(evt);
            var serializer = new CalendarSerializer(new SerializationContext());
            Console.Write(serializer.SerializeToString(cal));
        }

        /// <summary>
        /// Ensures that attendees can be properly removed from an event.
        /// </summary>
        [Test, Category("Attendee")]
        public void Remove1Attendee()
        {
            //ToDo: This test is broken as of 2016-07-13
            var evt = VEventFactory();
            Assert.AreEqual(0, evt.Attendees.Count);

            var attendee = _attendees[0];
            evt.Attendees.Add(attendee);
            Assert.AreEqual(1, evt.Attendees.Count);

            evt.Attendees.Remove(attendee);
            Assert.AreEqual(0, evt.Attendees.Count);
        }
    }
}