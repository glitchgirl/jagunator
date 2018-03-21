$(document).ready(function() {


        /* initialize the external events
        -----------------------------------------------------------------*/

        $('#external-events .fc-event').each(function() {

            // store data so the calendar knows to render an event upon drop
            $(this).data('event', {
                title: $.trim($(this).text()), // use the element's text as the event title
                stick: true // maintain when user navigates (see docs on the renderEvent method)
            });

            // make the event draggable using jQuery UI
            $(this).draggable({
                zIndex: 999,
                revert: true,      // will cause the event to go back to its
                revertDuration: 0  //  original position after the drag
            });

        });


        /* initialize the calendar
        -----------------------------------------------------------------*/

        $('#calendar').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            editable: true,
            droppable: true, // this allows things to be dropped onto the calendar
            dragRevertDuration: 0,
            drop: function() {
                // is the "remove after drop" checkbox checked?
                if ($('#drop-remove').is(':checked')) {
                    // if so, remove the element from the "Draggable Events" list
                    $(this).remove();
                }
            },
            eventDragStop: function( event, jsEvent, ui, view ) {
                
                if(isEventOverDiv(jsEvent.clientX, jsEvent.clientY)) {
                    $('#calendar').fullCalendar('removeEvents', event._id);
                    var el = $( "<div class='fc-event'>" ).appendTo( '#external-events-listing' ).text( event.title );
                    el.draggable({
                      zIndex: 999,
                      revert: true, 
                      revertDuration: 0 
                    });
                    el.data('event', { title: event.title, id :event.id, stick: true });
                }
            }
        });


        var isEventOverDiv = function(x, y) {

            var external_events = $( '#external-events' );
            var offset = external_events.offset();
            offset.right = external_events.width() + offset.left;
            offset.bottom = external_events.height() + offset.top;

            // Compare
            if (x >= offset.left
                && y >= offset.top
                && x <= offset.right
                && y <= offset .bottom) { return true; }
            return false;

        }


<<<<<<< HEAD
    });
=======
                });

                $('#calendar1').fullCalendar({
                    header: false,
                    //columnFormat: 'dddd',
                    editable: true,
                    droppable: true, 
                    dragRevertDuration: 0,
                    eventLimit: true, 
                    drop: function(date, jsEvent, ui) { console.log('calendar 1 drop'); console.log(date); console.log(jsEvent); console.log(ui); console.log(this);
                        /* is the "remove after drop" checkbox checked?*/
                        if ($('#drop-remove').is(':checked')) {
                            /* if so, remove the element from the "Draggable Events" list*/
                            $(this).remove();
                        }

                        /* if event dropped from another calendar, remove from that calendar*/
                        if (typeof calEventStatus['calendar'] != 'undefined') {
                            $(calEventStatus['calendar']).fullCalendar('removeEvents', calEventStatus['event_id']);
                            /*$(calEventStatus['calendar']).fullCalendar('unselect');*/
                        }

                        makeEventsDraggable();
                    },
                    eventReceive: function( event ) {  console.log('calendar 1 eventReceive');
                        makeEventsDraggable();
                    },
                    eventDrop: function( event, delta, revertFunc, jsEvent, ui, view ) {  console.log('calendar 1 eventDrop');
                        makeEventsDraggable();
                    },
                    eventDragStart: function( event, jsEvent, ui, view ) {
                        console.log(event); console.log(jsEvent); console.log(ui); console.log(view);

                        /* Add dragging event in global var*/ 
                        calEventStatus['calendar'] = '#calendar1';
                        calEventStatus['event_id'] = event._id;
                        console.log('calendar 1 eventDragStart');
                    },
                    eventDragStop: function( event, jsEvent, ui, view ) { console.log('calendar 1 eventDragStop');
                        
                        if(isEventOverDiv(jsEvent.clientX, jsEvent.clientY)) {
                            $('#calendar1').fullCalendar('removeEvents', event._id);
                            var el = $( "<div class='fc-event'>" ).appendTo( '#external-events-listing' ).text( event.title );
                            el.draggable({
                              zIndex: 999,
                              revert: true, 
                              revertDuration: 0 
                            });
                            el.data('event', { title: event.title, id :event.id, stick: true });
                        }

                        calEventStatus = []; 
                        makeEventsDraggable();
                    },
                    eventResize: function( event, delta, revertFunc, jsEvent, ui, view ) {
                        makeEventsDraggable();
                    },
                    viewRender: function() { console.log('calendar 1 viewRender');
                        makeEventsDraggable();
                    },
                });
            });
>>>>>>> 87215a67b0ffe2acafb726e57d58a8cc1d3bc26f
