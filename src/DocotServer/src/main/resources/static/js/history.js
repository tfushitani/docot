$(function(){
    var arg = new Object;
    var pair=location.search.substring(1).split('&');
    for(var i=0;pair[i];i++) {
        var kv = pair[i].split('=');
        arg[kv[0]]=kv[1];
    }

    var updatedWithin = 1000 * 3600 * 24 * 7;

    $.ajax({
           url: '/docot/v1/devices/' + arg.deviceId + '/',
           dataType: 'json',
           success: function(data) {
                $('#nicknameTitle').text(data.nickname);
           },
           error: function(e) {
               console.log(e.responseText);
           }
        });

    $.ajax({
           url: '/docot/v1/devices/' + arg.deviceId + '/histories/?updatedWithin=' + updatedWithin,
           dataType: 'json',
           success: function(data) {
               $('#devices').bootstrapTable({
                  data: data
               });
           },
           error: function(e) {
               console.log(e.responseText);
           }
        });
});

function dateFormat(value, row, index) {
    if (value) {
        //return moment(value).format('YYYY/MM/DD HH:mm:ss');
        return moment(value).format('MM/DD HH:mm');
    } else {
        return "-";
    }
}

function mapLinkFormat(value, row, index) {
    return "<a href='https://maps.google.com/maps?q=" + row.latitude + "," + row.longitude + "'>" + value + "</a>";
}



