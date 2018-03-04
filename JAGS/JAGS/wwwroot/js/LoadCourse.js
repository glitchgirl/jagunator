$(function () {
    $('.CourseList').on("change", function () {
        var selected = $(this).val() + '.csv';
        $(document).ready(function () {
            $.ajax({
                type: "GET",
                url: selected,
                dataType: "text",
                success: function (data) { LoadData(data); }
            });
        });

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



    })

});