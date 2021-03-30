var	studentconfig = {
	pageSize: 5,
	pageIndex: 1,
}
var studentController = {
	init: function () {
		studentController.GetStudent();
		studentController.registerEvent();
		studentController.GetFacultyID_NAME();
		studentController.validate();
	},
	registerEvent: function () {
		$(document).stop().on('click', '.btn-delete-Student', function () {
			var id = $(this).data('id');
			bootbox.confirm("Bạn có chắc chắn muốn xóa?", function (result) {
				if (result) {
					studentController.Delete(id);
				}
			})
		})
		$(document).stop().on('click', '.btn-edit-Student', function () {
			var id = $(this).data('id');
			var idfac = $(this).data('fac');
			$('#InfoUpdateStudent').modal('show');
			studentController.Detail(id);
			studentController.GetClassInFaculty(idfac);
		})
		$(document).stop().on('click', '#btnSave-InfoStudent', function () {
			if ($('#frmSaveDataStudent').valid()) {
				studentController.Save();
			}
		})
		$(document).stop().on('change', '#facultyname', function (e) {
			var optionSelected = $(this).find("option:selected");
			var id = optionSelected.data("idOptionfacul");
			$('#IDfacl').val(id);
			studentController.GetClassInFaculty(id);
		})
		$(document).stop().on('change', '#classname', function (e) {
			var optionSelected = $(this).find("option:selected");
			var id = optionSelected.data("idOptionclass");
			$('#IDclass').val(id);
		})
		$(document).stop().on('click', '.btn-info-Student', function () {
			var id = $(this).data('id');
			$('#InfoStudent').modal('show');
			studentController.GetCoursebyID(id);
		})
		$(document).stop().on('click', '.btn-delete-Course', function () {
			var idcourse = $(this).data('id');
			bootbox.confirm("Bạn có chắc chắn muốn xóa?", function (result) {
				if (result) {
					studentController.DeleteCoursebyID(idcourse);
				}
			})
		})
		$(document).stop().on('click', '#btnSearch', function () {
			studentController.GetStudent(true);
		})
		$(document).stop().on('keypress', '#txtSearch', function (e) {
			if (e.which == 13) {
				studentController.GetStudent(true);
			}
		})
		$(document).stop().on('click', '#btnAddUser', function () {
			studentController.UpLoad();
			studentController.GetStudent();
        })
	},
	validate: function () {
		$('#frmSaveDataStudent').validate({
			rules: {
				first_name: "required",
				last_name: "required",
				middle_name: "required",
				phone_no: "required",
				sex: "required",
				dob: "required",
				mail: "required",
				facultyname: "required",
				classname: "required",
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
				classname: "Thông tin lớp không được để trống",
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
		var classid = $('#IDclass').val();
		var InfoStudent = {
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
			CLASS_ID: classid,
		}
		$.ajax({
			url: '/Admin/Student/Save',
			data: InfoStudent,
			type: 'POST',
			dataType: 'json',
			success: function (response) {
				if (response.status == true) {
					bootbox.alert("Thành công", function () {
						$('#InfoUpdateStudent').modal('hide');
						studentController.GetStudent(true);
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
	GetStudent: function (changePageSize) {
		var name = $('#txtSearch').val();
		$.ajax({
			url: "/Admin/Student/GetStudent",
			type: 'GET',
			dataType: 'json',
			data: {
				name: name,
				page: studentconfig.pageIndex,
				pageSize: studentconfig.pageSize
			},
			success: function (response) {
				if (response.status) {
					var data = response.data;
					var html = '';
					var template = $('#data-Student').html();
					if (data != '' || name =='') {
						$.each(data, function (i, item) {
							html += Mustache.render(template, {
								IDFACULTY: item.FACULTY.ID,
								NAMEFACULTY: item.FACULTY.NAME,
								IDCLASS: item.CLASS.ID,
								NAMECLASS: item.CLASS.NAME,
								MAJORCLASS: item.CLASS.MAJOR,
								ID: item.ID,
								FIRST_NAME: item.LAST_NAME + ' ' + item.MIDDLE_NAME + ' ' + item.FIRST_NAME,
								PHONE_NO: item.PHONE_NO,
								MAIL: item.MAIL,
							});
							$('#tblData-Student').html(html);
							studentController.paging(response.total, function () {
								studentController.GetStudent();
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
		var totalPage = Math.ceil(totalRow / studentconfig.pageSize);

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
				studentController.GetStudent();
				studentconfig.pageIndex = page;
				setTimeout(callback, 200);
			}
		});
	},
	Delete: function (id) {
		$.ajax({
			url: '/Admin/Student/Delete',
			data: {
				id: id
			},
			type: 'POST',
			dataType: 'json',
			success: function (response) {
				if (response.status == true) {
					bootbox.alert("Thành công", function () {
						studentController.GetStudent(true);
					})
				}
			}
		});
	},
	Detail: function (id) {
		$.ajax({
			url: '/Admin/Student/Detail',
			data: {
				idstudent: id,
			},
			type: 'POST',
			dataType: 'json',
			success: function (response) {
				if (response.status == true) {
					var data = response.data[0];
					var datetime = studentController.convert(data.DoB);
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
					$('#facultyname').val(data.FACULTY.NAMEFACULTY);
					$('#IDfacl').val(data.FACULTY.ID);
					$('#classname').val(data.CLASS.NAMECLASS + '--' + data.CLASS.MAJOR);
					$('#IDclass').val(data.CLASS.ID);
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
	GetClassInFaculty: function (id) {
		$.ajax({
			url: '/Admin/Student/GetClassInFaculty',
			type: 'POST',
			data: {id:id},
			dataType: 'json',
			success: function (response) {
				var data = response.data[0];
				$('#classname').html('')
				for (var j = 0; j < data.CLASSes.length; j++) {		
					var optclass = new Option(data.CLASSes[j].NAMECLASS + '--' + data.CLASSes[j].MAJORCLASS);
					$(optclass).data('idOptionclass', data.CLASSes[j].IDCLASS);
					$('#classname').append(optclass);
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
				if ($('#facultyname').children('option').length < 1) {
					for (var i = 0; i < data.length; i++) {
						var opt = new Option(data[i].NAME);
						$(opt).data('idOptionfacul', data[i].ID);
						$('#facultyname').append(opt);
					}
                }
		
			}
		})
	},
	GetCoursebyID: function (id) {
		$.ajax({
			url: '/Admin/Student/GetCoursebyID',
			data: { idstudent: id },
			type: 'POST',
			datatype: 'json',
			success: function (response) {
				if (response.status == true) {
					var data = response.data;
					var html = '';
					var template = $('#data-InfoCourse').html();
					var student = $('#NameStudent').val(data[0].LAST_NAME + ' ' + data[0].MIDDLE_NAME + ' ' + data[0].FIRST_NAME);
					$.each(data[0].COURSE, function (i, item) {
						html += Mustache.render(template, {
							IDCOURSE: item.IDCOURSE,
							NAMECOURSE: item.NAMECOURSE,
							DESCRIPTION: item.DESCRIPTION,
							IDSEMESTER: item.SEMESTER.ID,
							SEMESTER: item.SEMESTER.TITLE,
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
					bootbox.alert("Thành công", function () {
						$('#InfoStudent').modal('hide');
					})
				}
			}
		})
	},
}
studentController.init();