

function showCardAddEvent() {
    renderFrmAddEvent(0, '#cardAddTopic')
}



function renderFrmAddEvent(topicId, position) {
    var template = $('#data-template-addEvent').html()
    var html = Mustache.render(template, {
        topicid: topicId
    });

    $(position + ' div[data-id="' + topicId + '"] div[name="frmAddEvent"]').html(html)

    let divdate = $(position + ' div[data-id="' + topicId + '"] div[name="frmAddEvent"] .frmAddEvent .date');
    let fromdate = divdate.children('input[name = "search-from-date"]');
    let todate = divdate.children('input[name = "search-to-date"]');

    $(fromdate).datetimepicker();
    $(todate).datetimepicker();

    CKEDITOR.replace('fullDesEvent' + topicId);

    $(position + ' div[data-id="' + topicId + '"] .AddTopic_btnAddEvent').click(function () {
        $(position + ' div[data-id="' + topicId + '"] div[name="frmAddEvent"] .frmAddEvent').toggleClass('d-none')
        $(position + ' div[data-id="' + topicId + '"] .divBtnAddEvent').toggleClass('d-none d-inline-flex')
    });
}



///////////////////////////////////////////////////////////////


function hideCardAddEvent(position) {
    $(position).on('click', 'div[name="frmAddEvent"] button[name="btn-CloseEvent"]', function () {
        $(this).closest('.frmAddEvent').toggleClass('d-none')
        $(this).parents('.card-body').children('div').children('.divBtnAddEvent').toggleClass('d-inline-flex d-none ')
    })
}

function hideCardAddEventInCardAddTopic() {
    hideCardAddEvent('#cardAddTopic')
}

function hideCardAddEventInCardTopicAdded() {
    hideCardAddEvent('#listTopic')
}

/////////////////////////////////////////////////////////////////////////////////

function addEventInCardAddTopic() {
    addEvent('#cardAddTopic')
}

function addEventInTopicAdded() {
    addEvent('#listTopic')
}


function addEvent(position) {


    $(position).on('click', 'div[name="frmAddEvent"] button[name="btn-AddEvent"]', function () {

        let topicid = $(this).closest('.card-body').data('id')
        $(position + ' div[data-id="' + topicid + '"] .frmAddEvent div.text-danger').remove()



        let title = $(position + ' div[data-id="' + topicid + '"] div[name="frmAddEvent"] .frmAddEvent input[name="AddEvent_title"]');
        let divdate = $(position + ' div[data-id="' + topicid + '"] div[name="frmAddEvent"] .frmAddEvent .date');
        let fromdate = divdate.children('input[name = "search-from-date"]');
        let todate = divdate.children('input[name = "search-to-date"]');

     


        if (title.val() == '' && (fromdate.val() == '' || todate.val() == '')) {
            $('<div class="text-danger">* Phải nhập tiêu đề</div>').insertAfter(title)
            $('<div class="text-danger">* Phải nhập đầy đủ thời gian</div>').insertAfter(divdate)
            return false;
        }
        else if (title.val() == '' && isInvalidTime(fromdate.val(), todate.val())) {
            $('<div class="text-danger">* Phải nhập tiêu đề</div>').insertAfter(title)
            return false;
        }
        else if (title.val() == '' && isInvalidTime(fromdate.val(), todate.val()) == false) {
            $('<div class="text-danger">* Phải nhập tiêu đề</div>').insertAfter(title)
            $('<div class="text-danger">* Thời gian kết thúc phải lớn hơn bắt đầu tối thiểu 5\'</div>').insertAfter(divdate)
            return false;
        }
        else if (fromdate.val() == '' || todate.val() == '') {
            $('<div class="text-danger">* Phải nhập đầy đủ thời gian</div>').insertAfter(divdate)
            return false;
        }
        else if (isInvalidTime(fromdate.val(), todate.val()) == false) {
            $('<div class="text-danger">* Thời gian bắt đầu phải lớn hơn thời gian hiện tại và nhỏ hơn kết thúc 4\'</div>').insertAfter(divdate)
            return false;
        }
        else {
            var valDesCKE = CKEDITOR.instances['fullDesEvent' + topicid].getData().slice(3, -4);           

            let flag = false;

            if (position == '#listTopic') {
                $.ajax({
                    url: '/Teacher/Subject/PostEvent',
                    type: 'POST',
                    data: { ID: generateID('event'), TITLE: title.val(), DESCRIPTION: valDesCKE, STARTDATE: fromdate.val(), DEADLINE: todate.val(), TOPIC_ID: topicid },
                    dataType: 'json',
                    async: false,
                    success: function (msg) {
                        if(msg.success) {
                            flag = true;
                            alert('Thêm thành công !')
                        }
                        else {
                            alert('Đã xảy ra lỗi !')
                        }
                    }
                });
            } else {
                flag = true;
            }
    
            if (flag) {
                var templateEvent = $('#data-template-event-item').html()
                var htmlItemEvent = Mustache.render(templateEvent, {
                    IdEvent: null,
                    TitleEvent: title.val(),
                    DesEvent: valDesCKE,
                    StartDate: fromdate.val(),
                    FinishDate: todate.val()
                });

                $(position + ' div[data-id="' + topicid + '"]  div[name="listEvent"]').append(htmlItemEvent)
                title.val('')
                CKEDITOR.instances['fullDesEvent' + topicid].setData('');
                fromdate.val('')
                todate.val('')
            }
         
            $(this).closest('.frmAddEvent').toggleClass('d-none')
            $(this).parents('.card-body').children('div').children('.divBtnAddEvent').toggleClass('d-inline-flex d-none ')
        }
    });
}


///////////////////////////////////////////////////////////////////////////////////////////////////////
function removeEventChildInCardAddtopic() {
    removeEventChild('#cardAddTopic')
}


function removeEventChildInTopicAdded() {
    removeEventChild('#listTopic')
}


function removeEventChild(position) {
    $(position).on('click', 'div[name="listEvent"]  .btn_RemoveEvent', function () {
        if (position == '#listTopic') {
            $.ajax({
                url: '/Teacher/Subject/DeleteEvent',
                dataType: 'json',
                type: 'POST',
                data: { id: $(this).closest('.itemEvent').data('id') },
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
        } 
        $(this).closest('.itemEvent').remove()
       
            
    });
}

////////////////////////////////////////////////////////////////////////////////////////////////////
function editEventChild(position) {

    $(position).on('click', 'div[name="listEvent"] .btn_EditEvent', function () {
        var eventID = $(this).closest('div.itemEvent').data('id')
        var item = $(this).parents('div[data-id="' + eventID + '"].itemEvent').children('.itemEventContent')

        item.toggleClass('d-none d-flex')
        var titleItem = item.children('h5').text();
        var desItem = item.children('p').text();
        var startDateItem = item.children('div.date').children('p[name="startDate"]').children('span').text();
        var finishDateItem = item.children('div.date').children('p[name="finishDate"]').children('span').text();

        var templateEditEvent = $('#data-template-editEvent-item').html()
        var editEventHtml = Mustache.render(templateEditEvent, {
            titleItem: titleItem,
            desItem: desItem,
            startDateItem: startDateItem,
            finishDateItem: finishDateItem
        });


        $(position).on('focus', 'div[name = "listEvent"] .formEdit_itemEvent input.dt', function () {
            $(this).datetimepicker();
        });

        $(this).parents('div[data-id="' + eventID + '"].itemEvent').append(editEventHtml)
    });
}


function editEventChildInTopicAdded() {
    editEventChild('#listTopic')
}

function editEventChildInCardAddtopic() {
    editEventChild('#cardAddTopic')
}



/////////////////////////////////////////////////////////////////////////////////////////////////////////

function closeFormEditEventChildInCardAddtopic() {
    closeFormEditEventChild('#cardAddTopic')
}

function closeFormEditEventChildInTopicAdded() {
    closeFormEditEventChild('#listTopic')
}

function closeFormEditEventChild(position) {

    $(position).on('click', 'div[name="listEvent"] .btn_close_EditEvent', function () {
        $(this).closest('div.itemEvent').children('.itemEventContent').toggleClass('d-none d-flex')
        $(this).closest(' .formEdit_itemEvent').remove();

    });
}



/////////////////////////////////////////////////////////////////////////////////////////////////
function editEventChildContentInCardAddTopic() {
    editEventChildContent('#cardAddTopic')
}
function editEventChildContentInTopicAdded() {
    editEventChildContent('#listTopic')
}



function editEventChildContent(position) {

    $(position).on('click', 'div[name = "listEvent"] .itemEvent .formEdit_itemEvent .btn_ComfirmEditEvent', function () {

        let eventID = $(this).closest('div.itemEvent').data('id')
        $(position + ' .itemEvent[data-id="' + eventID + '"] .formEdit_itemEvent div.text-danger').remove()

        let item = $(this).closest('.itemEvent[data-id= "' + eventID + '"] .formEdit_itemEvent')

        let itemStartDate = item.children('div.dateEdit').children('input[name="search-from-date"]')
        let itemFinishDate = item.children('div.dateEdit').children('input[name="search-to-date"]')

        if (item.children('.editEventTitle').val() == '' && (itemStartDate.val() == '' || itemFinishDate.val() == '')) {
            $('<div class="text-danger">* Không để trống</div>').insertAfter(item.children('.editEventTitle'))
            $('<div class="text-danger">* Không để trống</div>').insertAfter(item.children('div.dateEdit'))
            return false;
        }
        else if (item.children('.editEventTitle').val() == '' && isInvalidTime(itemStartDate.val(), itemFinishDate.val())) {
            $('<div class="text-danger">* Không để trống</div>').insertAfter(item.children('.editEventTitle'))
            return false;
        }
        else if (item.children('.editEventTitle').val() == '' && isInvalidTime(itemStartDate.val(), itemFinishDate.val()) == false) {
            $('<div class="text-danger">* Không để trống</div>').insertAfter(item.children('.editEventTitle'))
            $('<div class="text-danger">* Thời gian bắt đầu phải lớn hơn thời gian hiện tại và nhỏ hơn kết thúc 4\'</div>').insertAfter('div.dateEdit')
            return false;
        }
        else if (itemStartDate.val() == '' || itemFinishDate.val() == '') {
            $('<div class="text-danger">* Nhập đầy đủ thời gian</div>').insertAfter(item.children('div.dateEdit'))
            return false;
        }
        else if (isInvalidTime(itemStartDate.val(), itemFinishDate.val()) == false){
            $('<div class="text-danger">* Thời gian bắt đầu phải lớn hơn thời gian hiện tại và nhỏ hơn kết thúc 4\'</div>').insertAfter('div.dateEdit')
            return false;
        }
        else {
            var itemContent = $(this).parents('.itemEvent').children('.itemEventContent')
            let flag = false;
            if (position == '#listTopic') {
                $.ajax({
                    url: '/Teacher/Subject/UpdateEvent',
                    type: 'POST',
                    data: {
                        ID: eventID,
                        TITLE: item.children('.editEventTitle').val(),
                        DESCRIPTION: item.children('.editEventDes').val(),
                        STARTDATE: itemStartDate.val(),
                        DEADLINE: itemFinishDate.val(),
                        TOPIC_ID: $(this).closest('.card-body').data('id')
                    },
                    dataType: 'json',
                    async: false,
                    success: function (msg) {
                        if (msg.success) {
                            flag = true;
                            alert('Sửa thành công !')
                        }
                        else {
                            alert('Đã xảy ra lỗi !')
                        }
                    }
                });
            } else {
                flag = true;
            }

            if (flag) {
                itemContent.children('h5').text(item.children('.editEventTitle').val())
                itemContent.children('p').text(item.children('.editEventDes').val())
                itemContent.children('div.date').children('p[name="startDate"]').children('span').text(itemStartDate.val())
                itemContent.children('div.date').children('p[name="finishDate"]').children('span').text(itemFinishDate.val())
            } 
            item.remove()
            itemContent.toggleClass('d-none d-flex')
        }
    });
}


function isInvalidTime(a, b) {
    //__2020/11/29 18:00
    var yearA = parseInt(a.slice(0, 4));
    var monthA = parseInt(a.slice(5, 7));
    var dayA = parseInt(a.slice(8, 10));
    var hourA = parseInt(a.slice(11, 13));
    var minA = parseInt(a.slice(14));
    var dateA = new Date(yearA, monthA - 1, dayA, hourA, minA);

    var yearB = parseInt(b.slice(0, 4));
    var monthB = parseInt(b.slice(5, 7));
    var dayB = parseInt(b.slice(8, 10));
    var hourB = parseInt(b.slice(11, 13));
    var minB = parseInt(b.slice(14));
    var dateB = new Date(yearB, monthB - 1, dayB, hourB, minB);


    var offset = dateA.getTime() - Date.now();
    var offset2 = dateB.getTime() - dateA.getTime();
  
    
    if (offset2 > 1000 * 60 * 4 && offset >= 0) {
        return true;
    }
    return false;
}
