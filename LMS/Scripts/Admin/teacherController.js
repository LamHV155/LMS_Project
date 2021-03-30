var teacherconfig = {
	pageSize: 5,
	pageIndex: 1,
}
var teacherController = {
	init: function () {
		teacherController.GetTeacher();
		teacherController.registerEvent();
		teacherController.GetFacultyID_NAME();
		teacherController.validate();
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
		$(document).stop().on('change', '#facultyname', function (e) {
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
		$(document).stop().on('click', '#btnSearch', function () {
			teacherController.GetTeacher(true);
		})
		$(document).stop().on('keypress', '#txtSearch', function (e) {
			if (e.which == 13) {
				teacherController.GetTeacher(true);
			}
		})
		$(document).stop().on('click', '#btnAddUser', function () {
			teacherController.UpLoad();
			teacherController.GetTeacher(true);
		})
	},
	validate: function () {
		$('#frmSaveDataTeacher').validate({
			rules: {
				first_name: "required",
				last_name: "required",
				middle_name: "required",
				phone_no: "required",
				sex: "required",
				dob: "required",
				mail: "required",
				facultyname: "required",
			},
			messages: {
				first_name: "Tên không được để trống",
				last_name: "Tên không được để trống",
				middle_name: "Tên không được để trống",
				phone_no: "Số điện thoại không được để trống",
				sex: "Giới tính không được để trống",
				dob: "Ngày sinh không được để trống",
				mail: "Email không được để trống",
				facultyname: "Thông tin khoa không được để trống",
			},
		})
	},
	UpLoad: function () {
		var file = new FormData($('form').get(0));
		$.ajax({
			url: '/Admin/Student/UploadExcel',
			data: file,
			type: 'POST',
			contentType: false,
			processData: false,
			success: function (response) {
				if (response.status == true) {
					bootbox.alert("Thành công");
				}
				else if (response.status == false) {
					bootbox.alert("Không có file đính kèm");
				}
				else {
					bootbox.alert("Định dạng file không đúng")
				}
			},
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
					bootbox.alert("Thành công", function () {
						$('#InfoUpdateTeacher').modal('hide');
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
	GetTeacher: function (changePageSize) {
		var name = $('#txtSearch').val();
		$.ajax({
			url: '/Admin/Teacher/GetTeacher',
			type: 'GET',
			dataType: 'json',
			data: {
				name:name,
				page: teacherconfig.pageIndex,
				pageSize: teacherconfig.pageSize
			},
			success: function (response) {
				if (response.status) {
					var data = response.data;
					var html = '';
					var template = $('#data-Teacher').html();
					if (data != '' || name =='') {
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
							}, changePageSize);
						});
					}
					else {
						alert("Không có thông tin!")
                    }
				}
			}
		})
	},
	paging: function (totalRow, callback, changePageSize) {
		var totalPage = Math.ceil(totalRow / teacherconfig.pageSize);

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
							DESCRIPTION: item.COURSE.DESCRIPTION,
							IDSEMESTER: item.COURSE.SEMESTER.ID,
							SEMESTER: item.COURSE.SEMESTER.TITLE,
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