$(function ()
{
    $('#CourseList').change(function ()
    {
        if ($('#CourseList option:selected').text() !== '----None----')
        {
            //var selected = window.location.protocol + '//' + window.location.host + '/' + 'Home/Data/Courses/' + $('#CourseList option:selected').text() + '.csv';
            var selected = $('#CourseList option:selected').text();


            $.post("@Url.Action("GetCourseValues", "Home")?val=" + selected, function (res)
                {
                if (res.Success === "true") {
                    $('#InstructorName').val(res.data.InstructorName);
                    $('#CourseName').val(res.data.CourseName);
                    $('#CourseID').val(res.data.CourseID);
                    $('#CampusNames').val(res.data.CampusName);
                    $('#ClassroomStudentSize').val(res.data.ClassroomSize);
                }
            });


            //loadCSV(selected);
            /*
            function loadCSV(selected) {
                $.ajax({
                    type: "POST",
                    url: '/Data/Courses/' + selected,
                    dataType: "text",
                    success: function (data) { LoadData(data); },
                    error: function (xhr, ajaxOpotions, thrownError) {
                        alert("Status: " + xhr.status + "        Error: " + thrownError);
                    }
                }
            }
            */


            /*
            function LoadData(CourseInfo) {
                var AllText = CourseInfo.split(/\r\n|\n/);
                var headers = AllText[0].split(',');
                var info = [];
                var data;

                for (var i = 1; i < AllText.length; i++) {
                    data = AllText[i].split(',');
                }

                $('#InstructorName').val(data[0]);
                $('#CourseName').val(data[1]);
                $('#CourseID').val(data[2]);
                $('#CampusNames').val(data[3]);
                $('#ClassroomStudentSize').val(data[4]);
            }
                */


        }
    })

});