

function showCardAddTopic() {
    $('#btnAddTopic').click(function () {
        $('#cardAddTopic').toggleClass('d-none')
        $('#divBtnAddTopic').toggleClass('d-inline-flex d-none ')
        $('#loadingAddTopic').html('...');
    });
}

function hideCardAddTopic() {
    $('#btn-CloseAddTopic').click(function () {
        $('#cardAddTopic').toggleClass('d-none')
        $('#divBtnAddTopic').toggleClass('d-inline-flex d-none ')
        $('#loadingAddTopic').html('');
    });
}


function renderDataObjectIntoTopicTemplate(data) {

    var htmlTopic = ''

    var template = $('#data-template-topic-item').html()

    $.each(data, function (index, topic) {
        htmlTopic = Mustache.render(template, {
            IdTopic: topic.ID,
            TitleTopic: topic.TITLE,
            DesTopic: topic.DESCRIPTION
        });

        $('#listTopic').append(htmlTopic);
        var htmlDoc = ''
        var htmlEvent = ''

        var templateDoc = $('#data-template-doc-item').html()
        $.each(topic.DOCUMENTs, function (index, doc) {
            htmlDoc += Mustache.render(templateDoc, {
                IdDoc: doc.ID,
                fileName: doc.TITLE,
                LinkDoc: doc.LINK
            });
        });

        $('#listTopic div[data-id="' + topic.ID + '"] div[name="listDoc"]').html(htmlDoc)


        renderFrmAddDoc(topic.ID, '#listTopic')
        //////////////////////////////////////////

        var templateEvent = $('#data-template-event-item').html()
        $.each(topic.EVENTs, function (index, event) {

            let startDate = ''
            let deadline = ''
            if (event.STARTDATE.indexOf('Date') != -1) {
                 startDate = ConvertTimestampJSONToDateTime(event.STARTDATE)
                 deadline = ConvertTimestampJSONToDateTime(event.DEADLINE) 
            } else {
                 startDate = event.STARTDATE
                 deadline = event.DEADLINE
            }

            htmlEvent += Mustache.render(templateEvent, {
                IdEvent: event.ID,
                TitleEvent: event.TITLE,
                LinkEvent: event.LINK,
                DesEvent: event.DESCRIPTION,
                StartDate: startDate,
                FinishDate: deadline
            });
        });

        $('#listTopic div[data-id="' + topic.ID + '"] div[name="listEvent"]').html(htmlEvent)
        renderFrmAddEvent(topic.ID, '#listTopic')
        //alert($('#listTopic div[data-id="' + topic.ID + '"]').html())
    });
 
}

function getParamOfUrlSubject() {
    var url = new URL(window.location.href);
    var id = url.href.split('/');
    return id[6];
}


function PostAndRenderTopic() {

    $('#btn-AddTopic').on('click', function () {
        $('#cardAddTopic div.text-danger').remove()
        if ($('#AddTopic_title').val() == '') {
            $('<div class="text-danger">* Phải nhập tiêu đề</div>').insertAfter($('#AddTopic_title'))
            return false;
        }
        let data = {}
        data.data = []
        let topic = {};

        topic.ID = generateID('topic')
        topic.TITLE = $('#AddTopic_title').val();
        topic.DESCRIPTION = CKEDITOR.instances.fullDesTopic.getData().slice(3, -4);
        topic.COURSE_ID = getParamOfUrlSubject();
        //alert(JSON.stringify(topic))
        topic.DOCUMENTs = [];

        $.each($('#cardAddTopic div[name="listDoc"] .itemDoc .itemDocContent'), function (index, item) {
            //alert($(item).html())
            var doc = {}
            doc.ID = generateID('doc')
            doc.TITLE = $(item).children('a').children('p').text()
            doc.LINK = $(item).children('a').attr('href')
            //doc.DESCRIPTION = $(item).children('p').text()
            topic.DOCUMENTs.push(doc)
        });

        topic.EVENTs = [];
        $.each($('#cardAddTopic div[name="listEvent"] .itemEvent .itemEventContent'), function (index, item) {
            var event = {}
            event.ID = generateID('event')
            event.TITLE = $(item).children('h5').text()
            event.DESCRIPTION = $(item).children('p').text()
            event.STARTDATE = $(item).children('div.date').children('p[name="startDate"]').children('span').text()
            event.DEADLINE = $(item).children('div.date').children('p[name="finishDate"]').children('span').text()
            topic.EVENTs.push(event)
        });

        data.data.push(topic)


       


        $.ajax({
            url: '/Teacher/Subject/PostTopic',
            type: 'POST',
            async: true,
            dataType: 'json',
            data: topic,
            success: function (msg) {
                if (msg.success) {
                    renderDataObjectIntoTopicTemplate(data.data)
                    //----right bar----------------------
                    $('#listTopicRightbar').append('<a href="#' + topic.ID + '" style="color:blue; display:block; font-weight:400;">' + topic.TITLE + '</a>')
                        

                    $('#cardAddTopic').toggleClass('d-none')
                    $('#divBtnAddTopic').toggleClass('d-inline-flex d-none ')
                    $('#loadingAddTopic').html('');
                    $('#AddTopic_title').val('')
                    CKEDITOR.instances.fullDesTopic.setData('')
                    $('#cardAddTopic div[name="listDoc"]').html('')
                    $('#cardAddTopic div[name="listEvent"]').html('')
                    alert('Thêm thành công !')
                    return;
                }
                else {
                    alert("Đã xảy ra lỗi !")
                }
            }
        });

    });

  
}


function LoadListTopic(courseID) {
    $.ajax({
        url: '/Teacher/Subject/GetTopic',
        type: 'GET',
        dataType: 'json',
        data: { courseId: courseID },//userId: "JZDN2020112521542805", courseId: "JZDN2020112521542821", SubjectId: "JZDN2020112521542813"},
        async : true,
        success: function (response) {
            if (response.status) {
                renderDataObjectIntoTopicTemplate(JSON.parse(response.data))
                return;
            }
            else {
                window.location.href = response.data;
                return;
            }
        }
    })
    
    $.ajax({
        url: '/Teacher/Subject/GetDocInFormAddTopicFromCookie',
        type: 'GET',
        dataType: 'json',
        data: '',
        async: true,
        success: function (response) {
            if (response.status) {
                renderDocInCardAddTopic(response.data)
                return;
            }
            else {
                return;
            }
        }
    })
}

function renderDocInCardAddTopic(data) {
    var files = JSON.parse(data)
    var templateDoc = $('#data-template-doc-item').html()
    var htmlDoc = ''

    for (var i = 0; i < files.File.length; i++) {
        htmlDoc += Mustache.render(templateDoc, {
            //IdDoc: doc.ID,
            fileName: files.File[i].fileName,
            LinkDoc: files.File[i].linkFile
        });
    }
    $('#frm-add-topic div[name="listDoc"]').html(htmlDoc)
}

function ConvertTimestampJSONToDateTime(timestampJson) {
    let timestamp = timestampJson.slice(6, -2) 
    let date = new Date(parseInt(timestamp))
    return date.getFullYear() + '/' + ((date.getMonth() + 1) > 9 ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1)))
        + '/' + (date.getDate() > 9 ? date.getDate() : ('0' + date.getDate()))
        + ' ' + date.toLocaleTimeString('vi');
}


////////////////////////////////////////////////////////////

function removeTopic() {
    $('#listTopic').on('click', '.card-body button[name="removeTopic"]', function () {
        var id = $(this).closest('.card-body').data('id') 
        $('#listTopicRightbar a[href="#'+id+'"]').remove()
        $.ajax({
            url: '/Teacher/Subject/DeleteTopic',
            dataType: 'json',
            type: 'POST',
            data: { id: id},
            async: true,
            success: function (msg) {
                if (msg.success) {
                    alert('Xóa thành công !')
                } else {
                    alert('Đã xảy ra lỗi !')
                    return;
                }
            }
        });
        $(this).closest('.card').remove();
    });
}

