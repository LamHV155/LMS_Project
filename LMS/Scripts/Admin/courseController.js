var courseconfig = {
    pageSize: 5,
    pageIndex: 1,
}
var courseController = {
    init: function () {
        courseController.GetCourse();
        courseController.registerEvent();
        courseController.GetNameSemester();
        courseController.GetFacultyID_NAME();
        courseController.validate();
    },
    registerEvent: function () {
        $(document).stop().on('change', '#title', function (e) {
            var optionSelected1 = $(this).find("option:selected");
            var id1 = optionSelected1.data("idOptionsemester");
            $('#semester_id').val(id1);
        })
        $(document).stop().on('change', '#facultyname', function (e) {
            var optionSelected = $(this).find("option:selected");
            var id = optionSelected.data("idOption");
            $('#IDfacl').val(id);
            courseController.GetTeacherInFaculty(id);
            courseController.GetSubjectsInFaculty(id);
        })
        $(document).stop().on('change', '#teachername', function (e) {
            var optionSelected = $(this).find("option:selected");
            var id = optionSelected.data("idOptionteacher");
            $('#IDteacher').val(id);
        })
        $(document).stop().on('change', '#namesubject', function (e) {
            var optionSelected2 = $(this).find("option:selected");
            var id2 = optionSelected2.data("idOptionsubject");
            $('#subject_id').val(id2);
        })
        $(document).stop().on('click', '#btnSave-Course', function () {
            if ($('#frmSaveData-Course').valid()) {
                courseController.Save();
            }
        })
        $(document).stop().on('click', '.btn-delete-Course', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn chắc chắn muốn xóa", function (result) {
                if (result) {
                    courseController.Delete(id);
                }
            });
        })
        $(document).stop().on('click', '#btnAdd-Course', function () {
            $('#frmSaveData-Course').validate().resetForm();
            $('#CourseUpdateDetail').modal('show');
            courseController.reset();
           
        })
        $(document).stop().on('click', 'btnCancel-InfoTeacher', function (){
            $('#frmSaveData-Course').validate().resetForm();
        })
        $(document).stop().on('click', '.btn-delete-Student', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc muốn xóa?", function (result) {
                if (result) {
                    courseController.DeleteStudent(id);
                    $('#InfoCourse').modal('hide');
                }
            })
        })
        $(document).stop().on('click', '.btn-info-Course', function () {
            var id = $(this).data('id');
            $('#InfoCourse').modal('show');
            courseController.InfoCourse(id);
        })
        $(document).stop().on('click', '.btn-edit-Course', async function () {
            var id = $(this).data('id');
            var idfacl = $(this).data('facl')
            await courseController.GetTeacherInFaculty(idfacl);
            await courseController.GetSubjectsInFaculty(idfacl);
            courseController.Detail(id)
            $('#CourseUpdateDetail').modal('show');
        })
        $(document).stop().on('click', '#btnSearch', function () {
            courseController.GetCourse(true);
        })
        $(document).stop().on('keypress', '#txtSearch', function (e) {
            if (e.which == 13) {
                courseController.GetCourse(true);
            }
        })
    },
    validate: function () {
        $('#frmSaveData-Course').validate({
            rules: {
                idcourse: "required",
                coursename: "required",
                description: "required",
                semestername: "required",
                subjectname: "required",
                facultyname: "required",
                teachername: "required",
            },
            messages: {
                idcourse: "Mã học phần không được để trống",
                coursename: "Tên học phần không được để trống",
                description: "Mô tả không được để trống",
                semestername: "Vui lòng chọn khóa học",
                subjectname: "Vui lòng chọn môn học",
                facultyname: "Vui lòng chọn khoa",
                teachername: "Vui lòng chọn giảng viên",
            },
        })
    },
    GetCourse: function (changePageSize)
    {
        var name = $('#txtSearch').val();
        $.ajax({
            url: '/Course/GetCourse',
            type: 'GET',
            dataType: 'json',
            data: {
                name:name,
                page: courseconfig.pageIndex,
                pageSize: courseconfig.pageSize
            },
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-Course').html();
                    if (data != '' || name == '') {
                        $.each(data, function (i, item) {
                            html += Mustache.render(template, {
                                ID: item.ID,
                                NAME: item.NAME,
                                DESCRIPTION: item.DESCRIPTION,
                                IDFACULTY: item.SUBJECT.FACULTY_ID,
                            });
                        });
                        $('#tblData-Course').html(html);
                        courseController.paging(response.total, function () {
                            courseController.GetCourse();
                        }, changePageSize);
                    }
                    else {
                        alert("Không có thông tin")
                    }
                }
            },
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / courseconfig.pageSize);

        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                courseController.GetCourse();
                courseconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    Delete: function (id) {
        $.ajax({
            url: '/Course/Delete',
            data: { id: id },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Xóa thành công!", function () {
                        courseController.GetCourse(true);
                    })
                }
            }
        });
    },
    Detail: function (id) {
        $.ajax({
            url: '/Course/Detail',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json '
        }).done(function (response) {
            if (response.status == true) {
                var data = response.data[0];
                $('#IDcourse').attr('disabled', 'disabled');
                $('#IDcourse').val(data.IDCOURSE);
                $('#namecourse').val(data.NAMECOURSE);
                $('#description').val(data.DESCRIPTION);
                $('#semester_id').val(data.SEMESTER.ID);
                $('#title').val(data.SEMESTER.TITLE);
                $('#subject_id').val(data.SUBJECT.ID);
                $('#namesubject').val(data.SUBJECT.NAME);
                $('#IDfacl').val(data.TEACH.C_USER.FACULTY.ID);
                $('#facultyname').val(data.TEACH.C_USER.FACULTY.NAME);
                $('#IDteacher').val(data.TEACH.C_USER.ID);
                $('#teachername').val(data.TEACH.C_USER.LAST_NAME + ' ' + data.TEACH.C_USER.MIDDLE_NAME + ' ' + data.TEACH.C_USER.FIRST_NAME);
            }
        });
    },
    Save: function () {
        var idcourse=$('#IDcourse').val();
        var namecourse=$('#namecourse').val();
        var description=$('#description').val();
        var semester_id=$('#semester_id').val();
        var subject_id=$('#subject_id').val();
        var idteacher =$('#IDteacher').val();
        var file = new FormData($('form').get(0));
        file.append("ID", idcourse);
        file.append("NAME", namecourse);
        file.append("DESCRIPTION", description);
        file.append("SEMESTER_ID", semester_id);
        file.append("SUBJECT_ID", subject_id);

        file.append("USER_ID", idteacher);
        file.append("COURSE_ID", idcourse);

        $.ajax({
            url: '/Course/Save',
            data: file,
            contentType: false,
            processData: false,
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Lưu thành công.", function () {
                        $('#CourseUpdateDetail').modal('hide');
                        courseController.GetCourse(true);
                    })
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    GetNameSemester: function () {
        $.ajax({
            url: '/Admin/Course/GetNameSemester',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                for (var i = 0; i < data.length; i++) {
                    var optsemester = new Option(data[i].TITLE);
                    $(optsemester).data('idOptionsemester', data[i].ID);
                    $('#title').append(optsemester);
                }
     
            }
        })
    },
    GetFacultyID_NAME: function () {
        $.ajax({
            url: '/Admin/Teacher/GetFacultyID_NAME',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                for (var i = 0; i < data.length; i++) {
                    var opt = new Option(data[i].NAME);
                    $(opt).data('idOption', data[i].ID);
                    $('#facultyname').append(opt);
                }

            }
        })
    },
    GetSubjectsInFaculty: function (id) {
        return new Promise((res, rej) => {
            $.ajax({
                url: '/Admin/Course/GetSubjectsInFaculty',
                type: 'POST',
                data: { id: id },
                dataType: 'json',
                success: function (response) {
                    var data = response.data[0];
                    $('#namesubject').html('')
                    for (var j = 0; j < data.SUBJECTs.length; j++) {
                        var optsubject = new Option(data.SUBJECTs[j].NAMESUBS);
                        $(optsubject).data('idOptionsubject', data.SUBJECTs[j].IDSUBS);
                        $('#namesubject').append(optsubject);
                    }
                    res(true)
                }
            })
        })
    },
    GetTeacherInFaculty:  function (id) {
        return new Promise((res, rej) => {
            $.ajax({
                url: '/Admin/Course/GetTeacherInFaculy',
                type: 'POST',
                data: { id: id },
                dataType: 'json',
                success: function (response) {
                    var data = response.data[0];
                    $('#teachername').html('')
                    for (var j = 0; j < data.C_USER.length; j++) {
                        var optteacher = new Option(data.C_USER[j].LAST_NAME + ' ' + data.C_USER[j].MIDDLE_NAME + ' ' + data.C_USER[j].FIRST_NAME);
                        $(optteacher).data('idOptionteacher', data.C_USER[j].IDTEA);
                        $('#teachername').append(optteacher);
                    }
                    res(true)
                }
            })
        })
    },
    reset: function () {
        $('#IDcourse').removeAttr('disabled');
        $('#IDcourse').val('');
        $('#namecourse').val('');
        $('#description').val('');
        $('#semester_id').val('');
        $('#title').val('');
        $('#subject_id').val('');
        $('#namesubject').val('');
        $('#IDfacl').val('');
        $('#facultyname').val('');
        $('#IDteacher').val('');
        $('#teachername').val('');
    },
    InfoCourse: function (id) {
        $.ajax({
            url: '/Admin/Course/InfoCourse',
            data: { idcourse:id },
            dataType: 'json',
            type: 'POST',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-InfoStudent').html();
                    var teacher = $('#NameTeacher').val(data[0].TEACH.C_USER.LAST_NAME + ' ' + data[0].TEACH.C_USER.MIDDLE_NAME + ' ' + data[0].TEACH.C_USER.FIRST_NAME);
                    var course = $('#NameCourse').val(data[0].NAME);
                    $.each(data[0].C_USER, function (i, item) {
                        html += Mustache.render(template, {
                                ID: item.ID,
                                NAMESTUDENT: item.LAST_NAME + ' ' + item.MIDDLE_NAME + ' ' + item.FIRST_NAME,
                            });
                        });
                    $('#tblData-InfoStudent').html(html);
                }
            }
        })
    },
    DeleteStudent: function (id) {
        $.ajax({
            url: '/Course/DeleteStudent',
            data: { id: id },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Xóa thành công")
                }
            }
        });
    },
}
courseController.init();