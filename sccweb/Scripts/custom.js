//$(document).ready(function () {
    //$('#dtPagination').DataTable();
    //$('.dataTables_length').addClass('bs-select');
//});

$(function () {
    var i = 1;
    var ri = "";

    $('.texteditor').each(function () {
        var textId = $(this).attr('id');
        CKEDITOR.replace(textId);
    });

    $('.form-page-create input[type="submit"], .form-page-edit input[type="submit"]').click(function (e) {
        $('.texteditor:not(#Maintext)').each(function () {
            var textEditorId = $(this).attr('id'), ckEditorData = CKEDITOR.instances[textEditorId].getData(),
                dataReplace = ckEditorData.replace(',', '&comma;');
                CKEDITOR.instances[textEditorId].setData(dataReplace);
        });
    });

    $('.page .btn-add-mtext').click(function () {
        $('.form-multipletext').slideToggle();
    });

    $('.fi-data-img .fdi-remove').click(function () {
        $(this).parents('.fi-data-img').append('<input type="file" id="upload" name="upload" onchange="fileCheck(this)" />');
        $(this).siblings('.fdi-img').remove();
        $(this).remove();
    });

    $('.datepicker').datepicker({
        format: 'dd-mm-yy'
    });

    $('.page .ai-expand').click(function () {
        $(this).parents('.actions').siblings('.add-dtls').slideToggle();
        $(this).parents('tr').siblings('tr').toggleClass('overlay');
    });

    $('.textcontent').each(function () {
        var plaintext = $(this).text();
        $('.page-plaintext').append(plaintext);
        $(this).remove();
    });

    $('.temptext').each(function () {
        var ptext = $(this).text()
        $(this).siblings('.card-body').append(ptext);
        $(this).remove();
    });

    $('.add-dtls').each(function () {
        var pr = $(this).parents('.page').width();

        $(this).css('width', pr + 'px');
    });

    $('.btn-duplicate').click(function () {
        var container = $(this).data('container');   

        if ($(this).siblings(container).children('.fmi-sep:last-child').length == 1) {
            var dataLength = $(this).siblings(container).children('.fmi-sep:last-child').data('length');

            if (ri != "") {
                i = ri;
            } else {
                i = dataLength;
            }
        }

        i++;
        $(container).append('<div id="fmi-' + i + '" class="fm-item" data-fmitem="' + i + '"><input data-subid="' + i + '" type="text" name="subcontent" class="form-control fmi fmi-title" placeholder="Add Subtitle"/><textarea id="textEditor' + i + '" class="form-control texteditor fmi fmi-content" name="subcontent" data-conid="' + i + '" row="4" placeholder="Add Subcontent"></textarea><p class="text-right" style="margin:0; margin-top: 10px;"><span class="btn btn-sm btn-danger btn-remove-fmi">Remove</span></p></div>');
        CKEDITOR.replace('textEditor' + i);

        $('.btn-remove-fmi').click(function () {
            $(this).parents('.fm-item').remove();
        });
    });

    $('.btn-remove-fmisep').click(function () {
        if ($('.container').children('.fmi-sep:last-child').length == 1) {
            var dataLength = $('.container').children('.fmi-sep:last-child').data('length');
            ri = dataLength;
        }
        var dataItem = $(this).data('remove');

        $(this).parents('.fmi').siblings('.fmi[data-item="' + dataItem + '"]').remove();
        $(this).parents('.fmi').remove();
    });

    //--- Start Button Toggle Class
    $('.btn-toggle').click(function () {
        var target = $(this).data('target'), toggleClass = $(this).data('toggle');
        $(target).toggleClass(toggleClass);
    });

    $('.btn.collapse-hidden').click(function () {
        var parentId = $(this).data('parentid'), toggleId = $(this).data('target');
        $(toggleId).attr('data-content', '1');
        $(parentId).attr('data-content', '0');
        $(parentId).slideToggle();
        $(toggleId).slideToggle();
    });
    //--- End Button Toggle Class

    //--- Start Background Image Block
    $('.bg-img').each(function () {
        var target = $(this).data('target'), imgsrc = $(this).find(target).attr('src');
        $(this).attr('style', 'background-image: url(' + imgsrc + ')');
    });
    //--- End Background Image Block
});