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

// Toast helper using Bootstrap 5
window.toast = {
    show: function (message, type) {
        try {
            var variant = 'success';
            switch ((type || '').toLowerCase()) {
                case 'error': variant = 'danger'; break;
                case 'warning': variant = 'warning'; break;
                case 'info': variant = 'info'; break;
                default: variant = 'success'; break;
            }

            var container = document.getElementById('toast-container');
            if (!container) {
                container = document.createElement('div');
                container.id = 'toast-container';
                container.className = 'toast-container position-fixed top-0 end-0 p-3';
                document.body.appendChild(container);
            }

            var toastEl = document.createElement('div');
            // Use bg/text utilities for broader Bootstrap compatibility
            var bgClass = 'bg-success';
            var textClass = 'text-white';
            var closeClass = 'btn-close-white';
            switch (variant) {
                case 'danger':
                    bgClass = 'bg-danger';
                    textClass = 'text-white';
                    closeClass = 'btn-close-white';
                    break;
                case 'warning':
                    bgClass = 'bg-warning';
                    textClass = 'text-dark';
                    closeClass = 'btn-close';
                    break;
                case 'info':
                    bgClass = 'bg-info';
                    textClass = 'text-white';
                    closeClass = 'btn-close-white';
                    break;
                default:
                    bgClass = 'bg-success';
                    textClass = 'text-white';
                    closeClass = 'btn-close-white';
                    break;
            }
            toastEl.className = 'toast align-items-center ' + bgClass + ' ' + textClass + ' border-0 shadow';
            toastEl.setAttribute('role', 'alert');
            toastEl.setAttribute('aria-live', 'assertive');
            toastEl.setAttribute('aria-atomic', 'true');
            toastEl.innerHTML = '<div class="d-flex">' +
                '<div class="toast-body">' + (message || '') + '</div>' +
                '<button type="button" class="' + closeClass + ' me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>' +
                '</div>';

            container.appendChild(toastEl);

            if (window.bootstrap && window.bootstrap.Toast) {
                var toast = new window.bootstrap.Toast(toastEl, { delay: 4000 });
                toast.show();
                toastEl.addEventListener('hidden.bs.toast', function () {
                    toastEl.remove();
                });
            } else {
                // Fallback
                console.log('Toast:', type, message);
            }
        } catch (e) {
            console.error('Toast error', e);
        }
    }
};