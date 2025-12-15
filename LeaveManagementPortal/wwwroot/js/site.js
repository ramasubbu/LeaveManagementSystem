window.blazorDate = {
    fixDateTimeLocal: function (element) {
        if (element && element.type === 'date') {
            element.addEventListener('change', function () {
                // Get the selected date and ensure it's treated as local
                var selectedDate = new Date(element.value + 'T00:00:00');
                
                // Format as ISO string but keep local time
                var year = selectedDate.getFullYear();
                var month = String(selectedDate.getMonth() + 1).padStart(2, '0');
                var day = String(selectedDate.getDate()).padStart(2, '0');
                
                element.value = year + '-' + month + '-' + day;
            });
        }
    },
    
    setDateValue: function (element, dateValue) {
        if (element && element.type === 'date' && dateValue) {
            var date = new Date(dateValue);
            var year = date.getFullYear();
            var month = String(date.getMonth() + 1).padStart(2, '0');
            var day = String(date.getDate()).padStart(2, '0');
            
            element.value = year + '-' + month + '-' + day;
        }
    }
};