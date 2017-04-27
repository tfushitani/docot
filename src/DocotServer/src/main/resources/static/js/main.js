$(function(){
    $.ajax({
           url: '/docot/v1/devices/?positionUpdatedWithin=86400000',
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
        return moment(value).format('HH:mm');
    } else {
        return "-";
    }
}
