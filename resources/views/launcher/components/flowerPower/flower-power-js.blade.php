<script>
    setTimeout(function() {
        $('[data-toggle="tooltip"]').tooltip();
        
        $(document).on("click", ".log-out-button", function() {
           location.href = "{{ route('logOut') }}";
        });

        $(document).on("click", '.settings-button', function(){
            $('#settings-modal').modal('show')
        })
    }, 1500);

    function cargarFlowerPower() {
        addLogOutButton();

        function addLogOutButton() {
            $(".settings-button").removeClass("none");
            $(".settings-button").addClass("block");
        }
    }
</script>
