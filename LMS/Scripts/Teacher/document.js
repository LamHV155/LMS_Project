

function showCardAddDocInCardAddTopic() {
    renderFrmAddDoc('0', '#cardAddTopic')
}


function renderFrmAddDoc(topicId, position) {

    var template = $('#data-template-addDoc').html()
    if (position == '#cardAddTopic') {
        var html = Mustache.render(template, {
            action: '/Teacher/Subject/UploadFileFromFormAddTopic',
            topicid: topicId
        });
    }
    else {
        var html = Mustache.render(template, {
            action: '/Teacher/Subject/PostDoc',
            topicid: topicId
        });
    }
    


    $(position + ' div[data-id="' + topicId + '"] div[name="frmAddDoc"]').html(html)
    $(position + ' div[data-id="' + topicId + '"] .AddTopic_btnAddDoc').click(function () {
        $(position + ' div[data-id="' + topicId + '"] div[name="frmAddDoc"] .frmAddDoc').toggleClass('d-none')
        $(position + ' div[data-id="' + topicId + '"] .divBtnAddDoc').toggleClass('d-none d-inline-flex')
    });
}



//////////////////////////////////////////////////////////////////////////

function hideCardAddDocInCardAddTopic() {
    hideCardAddDoc('#cardAddTopic')
}

function hideCardAddDocInCardTopicAdded() {
    hideCardAddDoc('#listTopic')
}


function hideCardAddDoc(position) {
    $(position).on('click', 'div[name="frmAddDoc"] button[name="btn-CloseDoc"]', function () {
        $(this).closest('.frmAddDoc').toggleClass('d-none')
        $(this).parents('.card-body').children('div').children('.divBtnAddDoc').toggleClass('d-inline-flex d-none ')
    })
}




//////////////////////////////////////////////////////////////////////////////////
function addDocInCardAddTopic() {
    addDoc('#cardAddTopic')
}

function addDocInCardTopicAdded() {
    addDoc('#listTopic')
}

function addDoc(position) {
    
    $(position).on('click', 'div[name="frmAddDoc"] button[name="btn-AddDoc"]', function () {
        let topicid = $(this).closest('.card-body').data('id')
        $(position + ' div[data-id="' + topicid +'"] .frmAddDoc div.text-danger').remove()

        let link = $(position + ' div[data-id="' + topicid +'"] div[name="frmAddDoc"] .frmAddDoc input[name="AddDoc_link"]')
        if (link.val() == '') {
            $('<div class="text-danger">* Phải thêm tài liệu</div>').insertAfter(link)
            return false;
        }
        else{
            let flag = false;
            let linkFile = '';
            let filename = '';
           // alert(JSON.stringify($('#formDoc' + topicid).get(0)))
            
            var formdata = new FormData($('form#formDoc' + topicid).get(0));
            
            let docid = generateID('doc');
            formdata.append('DocID', docid);

           // alert(JSON.stringify(formdata))
            if (position == '#listTopic') {
                formdata.append('TopicID', topicid);
                $.ajax({
                    type: 'POST',
                    url: '/Teacher/Subject/PostDoc',
                    data: formdata,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    async: false,
                    success: function (msg) {
                        if (msg.status == true) {
                            flag = true;
                            linkFile = msg.file.link;
                            filename = msg.file.filename;
                        } else {
                            alert('Đã xảy ra lỗi !')
                        }
                    }
                });
            } else {// trong cardAddTopic
                flag = true;
                $.ajax({
                    type: 'POST',
                    url: '/Teacher/Subject/UploadFileFromFormAddTopic',
                    data: formdata,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    async: false,
                    success: function (msg) {
                        if (msg.status == true) {
                            flag = true;
                            linkFile = msg.file.link;
                            filename = msg.file.filename;
                        } else {
                            alert('Đã xảy ra lỗi !')
                        }
                    }
                });
            }

            if (flag) {
                var templateDoc = $('#data-template-doc-item').html()
                var htmlItemDoc = Mustache.render(templateDoc, {
                    IdDoc: docid,
                    fileName: filename,
                    LinkDoc: linkFile
                });
                $(position + ' div[data-id="' + topicid + '"]  div[name="listDoc"]').append(htmlItemDoc)
                link.val('')
                alert('Thêm thành công !')
            }

            $(this).closest('.frmAddDoc').toggleClass('d-none')
            $(this).parents('.card-body').children('div').children('.divBtnAddDoc').toggleClass('d-inline-flex d-none ')
        }

    });
}


//////////////////////////////////////////////////////////////////////////////

function removeDocChild(position) {
   
    $(position).on('click', 'div[name="listDoc"]  .btn_RemoveDoc', function () {
        if (position == '#listTopic') {
            $.ajax({
                url: '/Teacher/Subject/DeleteDoc',
                dataType: 'json',
                type: 'POST',
                data: { id: $(this).closest('.itemDoc').data('id') },
                async: true,
                dataType: 'json',
                success: function (msg) {
                    if (msg.status) {
                        alert('Xóa thành công !')
                    } else {
                        alert('Đã xảy ra lỗi !')
                        return;
                    }
                }
            });
        }
        else {
            $.ajax({
                url: '/Teacher/Subject/DeleteFileFromFormAddTopic',
                dataType: 'json',
                type: 'POST',
                data: {
                    title: $(this).closest('.itemDocContent').children('a').children('p').text(),
                    link: $(this).closest('.itemDocContent').children('a').attr('href')
                },
                async: true,
                dataType: 'json',
                success: function (msg) {
                    if (msg.status) {
                        //alert('Xóa thành công !')
                    } else {
                        alert('Đã xảy ra lỗi !')
                        return;
                    }
                }
            });
        }
        $(this).closest('.itemDoc').remove()
       
    });
}

function removeDocChildInCardAddtopic() {
    removeDocChild('#cardAddTopic')
}


function removeDocChildInTopicAdded() {
    removeDocChild('#listTopic')
}


////////////////////////////////////////

function editDocChildInCardAddtopic() {
    editDocChild('#cardAddTopic')
}

function editDocChildInTopicAdded() {
    editDocChild('#listTopic')
}

function editDocChild(position) {

    $(position).on('click', 'div[name="listDoc"] .btn_EditDoc', function () {

        var docID = $(this).closest('div.itemDoc').data('id')
        var item = $(this).parents('div[data-id="' + docID + '"].itemDoc').children('.itemDocContent')


        item.toggleClass('d-none d-flex')

        var templateEditDoc = $('#data-template-editDoc-item').html()
        var editDocHtml = Mustache.render(templateEditDoc, {
        });      
        $(this).parents('div[data-id="' + docID +'"].itemDoc').append(editDocHtml)
    });
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function closeFormEditDocChildInCardAddtopic() {
    closeFormEditDocChild('#cardAddTopic')
}

function closeFormEditDocChildInTopicAdded() {
    closeFormEditDocChild('#listTopic')
}

function closeFormEditDocChild(position) {

    $(position).on('click', 'div[name="listDoc"] .btn_close_EditDoc', function () {
        $(this).closest('div.itemDoc').children('.itemDocContent').toggleClass('d-none d-flex')
        $(this).closest(' .formEdit_itemDoc').remove();

    });
}




///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function editDocChildContentInCardAddTopic() {
    editDocChildContent('#cardAddTopic')   
}
function editDocChildContentInTopicAdded() {
    editDocChildContent('#listTopic')
}


function editDocChildContent(position) {
   
    $(position).on('click', 'div[name = "listDoc"] .itemDoc .formEdit_itemDoc .btn_ComfirmEditDoc', function () {

        let docID = $(this).closest('div.itemDoc').data('id')
        //alert($(this).closest('div.itemDoc').html())

        $(position + ' .itemDoc[data-id="'+docID+'"] .formEdit_itemDoc div.text-danger').remove()

        var item = $(this).closest('.itemDoc[data-id= "'+docID+'"] .formEdit_itemDoc')
        //alert(item.html())
        if (item.children('.editDocLink').val() == '') {
            $('<div class="text-danger">* Không để trống</div>').insertAfter(item.children('.editDocLink'))
            return false;
        }
        else {
            var itemContent = $(this).closest('.itemDoc').children('.itemDocContent')
            let flag = false;
            let link = '';
            let filename = '';
            let formdata = new FormData(item.get(0))
       
            if (position == '#listTopic') {
                formdata.append('DocID', docID);
                $.ajax({
                    type: 'POST',
                    url: '/Teacher/Subject/UpdateDoc',
                    data: formdata,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    async: false,
                    success: function (msg) {
                        if (msg.status == true) {
                            flag = true;
                            linkFile = msg.file.link;
                            filename = msg.file.filename;
                           // alert('Sửa thành công !')
                        } else {
                            alert('Đã xảy ra lỗi !')
                        }
                    }
                });
            } else {// trong cardAddTopic
                formdata.append('link', itemContent.children('a').attr('href'))
                $.ajax({
                    type: 'POST',
                    url: '/Teacher/Subject/UpdateDocFromFormAddTopic',
                    data: formdata,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    async: false,
                    success: function (msg) {
                        if (msg.status == true) {
                            flag = true;
                            linkFile = msg.file.link;
                            filename = msg.file.filename;
                            //alert('Sửa thành công !')
                        } else {
                            alert('Đã xảy ra lỗi !')
                        }
                    }
                });
            }

            if (flag) {
                itemContent.children('a').attr('href', linkFile)
                itemContent.children('a').children('p').text(filename)
               // itemContent.children('h5').text(item.children('.editDocTitle').val())
                //itemContent.children('p').text(item.children('.editDocDes').val())
            }
            item.remove()
            itemContent.toggleClass('d-none d-flex')
        }
    });
}




