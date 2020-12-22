﻿var teacherconfig = {
	pageSize: 5,
	pageIndex: 1,
}
var teacherController = {
	init: function () {
		teacherController.GetTeacher();
		teacherController.registerEvent();
		teacherController.GetFacultyID_NAME();
	},
	registerEvent: function () {
		$(document).stop().on('click', '.btn-delete-Teacher', function () {
			var id = $(this).data('id');
			bootbox.confirm("Bạn có chắc chắn muốn xóa?", function (result) {
				if (result) {
					teacherController.Delete(id);
				}
			})
		})
		$(document).stop().on('click', '.btn-edit-Teacher', function () {
			var id = $(this).data('id');
			$('#InfoUpdateTeacher').modal('show');
			teacherController.Detail(id);
		})
		$(document).stop().on('click', '#btnSave-InfoTeacher', function () {
			if ($('#frmSaveDataTeacher').valid()) {
				teacherController.Save();
			}
		})
		$(document).stop().on('click', '#facultyname', function (e) {
			var optionSelected = $(this).find("option:selected");
			var id = optionSelected.data("idOption");
			$('#IDfacl').val(id);
		})
		$(document).stop().on('click', '.btn-info-Teacher', function () {
			var id = $(this).data('id');
			$('#InfoTeacher').modal('show');
			teacherController.GetCoursebyID(id);
		})
		$(document).stop().on('click', '.btn-delete-Course', function () {
			var idcourse = $(this).data('id');
			bootbox.confirm("Bạn có chắc chắn muốn xóa?", function (result) {
				if (result) {
					teacherController.DeleteCoursebyID(idcourse);
                }
            })
        })
	},
	Save: function () {
		var id = $('#ID').val();
		var first = $('#first_name').val();
		var last = $('#last_name').val();
		var middle = $('#middle_name').val();
		var phone = $('#phone_no').val();
		var sex = $('#sex').val();
		var dob = $('#dob').val();
		var mail = $('#mail').val();
		var password = $('#password').val();
		var lastvisited = $('#lastvisited').val();
		var facultyid = $('#IDfacl').val();
		var InfoTeacher = {
			ID: id,
			FIRST_NAME: first,
			LAST_NAME: last,
			MIDDLE_NAME: middle,
			PHONE_NO: phone,
			SEX: sex,
			DoB: dob,
			MAIL: mail,
			PASSWORD: password,
			LASTVISITDATE: lastvisited,
			FACULTY_ID: facultyid,
		}
		$.ajax({
			url: '/Admin/Teacher/Save',
			data: InfoTeacher,
			type: 'POST',
			dataType: 'json',
			success: function (response) {
				if (response.status == true) {
					bootbox.alert("Sửa thành công!", function () {
						$('#frmSaveDataTeacher').modal('hide');
						teacherController.GetTeacher(true);
					})
				} else {
					bootbox.log(response.message);
				}
			},
			error: function (err) {
				console.log(err);
			}
		})
	},
	GetTeacher: function () {
		$.ajax({
			url: '/Admin/Teacher/GetTeacher',
			type: 'GET',
			dataType: 'json',
			data: {
				page: teacherconfig.pageIndex,
				pageSize: teacherconfig.pageSize
			},
			success: function (response) {
				if (response.status) {
					var data = response.data;
					var html = '';
					var template = $('#data-Teacher').html();
					$.each(data, function (i, item) {
						html += Mustache.render(template, {
							IDNAMEFACULTY: item.FACULTY.ID,
							NAMEFACULTY: item.FACULTY.NAME,
							ID: item.ID,
							FIRST_NAME: item.LAST_NAME + ' ' + item.MIDDLE_NAME + ' ' + item.FIRST_NAME,
							PHONE_NO: item.PHONE_NO,
							MAIL: item.MAIL,
						});
						$('#tblData-Teacher').html(html);
						teacherController.paging(response.total, function () {
							teacherController.GetTeacher();
						});
						teacherController.registerEvent();
					});
				}
			}
		})
	},
	paging: function (totalRow, callback) {
		var totalPage = Math.ceil(totalRow / teacherconfig.pageSize);

		////Unbind pagination if it existed or click change pagesize
		//if ($('#pagination a').length === 0 || changePageSize === true) {
		//    $('#pagination').empty();
		//    $('#pagination').removeData("twbs-pagination");
		//    $('#pagination').unbind("page");
		//}

		$('#pagination').twbsPagination({
			totalPages: totalPage,
			first: "Đầu",
			next: "Tiếp",
			last: "Cuối",
			prev: "Trước",
			visiblePages: 10,
			onPageClick: function (event, page) {
				teacherController.GetTeacher();
				teacherconfig.pageIndex = page;
				setTimeout(callback, 200);
			}
		});
	},
	Delete: function (id) {
		$.ajax({
			url: '/Admin/Teacher/Delete',
			data: {
				id: id
			},
			type: 'POST',
			dataType: 'json',
			success: function (response) {
				if (response.status == true) {
					bootbox.alert("Xóa thành công", function () {
						teacherController.GetTeacher(true);
					})
				}
			}
		});
	},
	Detail: function (id) {
		$.ajax({
			url: '/Admin/Teacher/Detail',
			data: {
				idnameteacher: id,
			},
			type: 'POST',
			dataType: 'json',
			success: function (response) {
				if (response.status == true) {
					var data = response.data[0];
					var datetime = teacherController.convert(data.DoB);
					$('#ID').val(data.ID);
					$('#first_name').val(data.FIRST_NAME);
					$('#last_name').val(data.LAST_NAME);
					$('#middle_name').val(data.MIDDLE_NAME);
					$('#phone_no').val(data.PHONE_NO);
					$('#sex').val(data.SEX);
					$('#dob').val(datetime);
					$('#mail').val(data.MAIL);
					$('#password').val(data.PASSWORD);
					$('#lastvisited').val(data.LASTVISITED);
					$('#facultyname').val(data.FACULTY.NAME);
					$('#IDfacl').val(data.FACULTY.ID);
				}
			}
		})
	},
	convert: function ConvertTimestampJSONToDateTime(timestampJson) {
		let timestamp = timestampJson.slice(6, -2)
		let date = new Date(parseInt(timestamp))
		return date.getFullYear() + '/' + ((date.getMonth() + 1) > 9 ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) +
			'/' + (date.getDate() > 9 ? date.getDate() : ('0' + date.getDate())) +
			' ' + date.toLocaleTimeString('vi');
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
	GetCoursebyID: function (id) {
		$.ajax({
			url: '/Admin/Teacher/GetCoursebyID',
			data: {idteacher:id},
			type: 'POST',
			datatype: 'json',
			success: function (response) {
				if (response.status == true) {
					var data = response.data;
					var html = '';
					var template = $('#data-InfoCourse').html();
					var teacher = $('#NameTeacher').val(data[0].LAST_NAME + ' ' + data[0].MIDDLE_NAME + ' ' + data[0].FIRST_NAME);
					$.each(data[0].TEACHES, function (i, item) {
						html += Mustache.render(template, {
							IDCOURSE: item.COURSE.ID,
							NAMECOURSE: item.COURSE.NAME,
							IDSEMESTER: item.COURSE.SEMESTER.ID,
							SEMESTER: item.COURSE.SEMESTER.TILTE,
						});
					});
					$('#tblData-Course').html(html);
                }
            }
        })
	},
	DeleteCoursebyID: function (idcourse) {
		$.ajax({
			url: '/Admin/Teacher/DeleteCoursebyID',
			data: {
				idcourse: idcourse
			},
			type: 'POST',
			dataType: 'json',
			success: function (response) {
				if (response.status == true) {
					bootbox.alert("Xóa thành công", function () {
						$('#InfoTeacher').modal('hide');
					})
                }
            }
        })
    }
}
teacherController.init();