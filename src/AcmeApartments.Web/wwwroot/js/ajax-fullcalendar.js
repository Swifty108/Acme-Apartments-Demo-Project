////$(document).ready(function () {
////    var events = [];

////    $.ajax({
////        type: "GET",
////        url: "/manager/getschedulerevents",
////        success: function (data) {
////            $.each(data, function (i, e) {
////                events.push({
////                    title: e.Title,
////                    notes: e.Notes,
////                    color: e.Color,
////                    start: moment(e.Start),
////                    end: v.End != null ? moment(e.End) : null
////                });
////            })

////            GenerateCalender(events);
////        },
////        error: function (error) {
////            alert('failed');
////        }
////    })

////    function GenerateCalender(events) {
////        $('#calender').fullCalendar('destroy');
////        $('#calender').fullCalendar({
////            contentHeight: 400,
////            defaultDate: new Date(),
////            timeFormat: 'h(:mm)a',
////            header: {
////                left: 'prev,next today',
////                center: 'title',
////                right: 'month,basicWeek,basicDay,agenda'
////            },
////            eventLimit: false,
////            editable: true,
////            eventColor: '#378006',
////            events: events,
////            eventClick: function (calEvent, jsEvent, view) {
////                $('#myModal #eventTitle').text(calEvent.title);
////                var $description = $('<div/>');
////                $description.append($('<p/>').html('<b>Start:</b>' + calEvent.start.format("MMM-DD-YYYY HH:mm a")));
////                if (calEvent.end != null) {
////                    $description.append($('<p/>').html('<b>End:</b>' + calEvent.end.format("MMM-DD-YYYY HH:mm a")));
////                }
////                $description.append($('<p/>').html('<b>Description:</b>' + calEvent.description));
////                $('#myModal #pDetails').empty().html($description);

////                $('#myModal').modal();
////            }
////        })
////    }
////})

document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        headerToolbar: {
            left: 'prevYear,prev,next,nextYear today',
            center: 'title',
            right: 'dayGridMonth,dayGridWeek,dayGridDay'
        },
        initialDate: '2020-09-12',
        navLinks: true, // can click day/week names to navigate views
        editable: true,
        dayMaxEvents: true, // allow "more" link when too many events
        events: [
            {
                title: 'All Day Event',
                start: '2020-09-01'
            },
            {
                title: 'Long Event',
                start: '2020-09-07',
                end: '2020-09-10'
            },
            {
                groupId: 999,
                title: 'Repeating Event',
                start: '2020-09-09T16:00:00'
            },
            {
                groupId: 999,
                title: 'Repeating Event',
                start: '2020-09-16T16:00:00'
            },
            {
                title: 'Conference',
                start: '2020-09-11',
                end: '2020-09-13'
            },
            {
                title: 'Meeting',
                start: '2020-09-12T10:30:00',
                end: '2020-09-12T12:30:00'
            },
            {
                title: 'Lunch',
                start: '2020-09-12T12:00:00'
            },
            {
                title: 'Meeting',
                start: '2020-09-12T14:30:00'
            },
            {
                title: 'Happy Hour',
                start: '2020-09-12T17:30:00'
            },
            {
                title: 'Dinner',
                start: '2020-09-12T20:00:00'
            },
            {
                title: 'Birthday Party',
                start: '2020-09-13T07:00:00'
            },
            {
                title: 'Click for Google',
                url: 'http://google.com/',
                start: '2020-09-28'
            }
        ]
    });

    calendar.render();
});