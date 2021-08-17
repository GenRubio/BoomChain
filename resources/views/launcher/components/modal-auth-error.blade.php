<div class="modal fade" id="auth-error-launcher" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle"
    aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLongTitle">Modal title</h5>
            </div>
            <div class="modal-body">
                <form id="login-metamask">
                    <div class="form-group">
                        <label for="exampleInputPassword1">Metamask</label>
                        <input type="password" class="form-control" id="exampleInputPassword1" placeholder="Metamask account ID">
                    </div>
                    <button type="submit" class="btn btn-primary">Login</button>
                </form>
            </div>
            <div class="modal-footer">
                <a type="button" href="{{ route('home') }}" target="_blank" class="btn btn-primary">Go to web</a>
            </div>
        </div>
    </div>
</div>

<script>
    setTimeout(function() {
        $(document).ready(function() {
            $('#auth-error-launcher').modal({
                backdrop: 'static',
                keyboard: false
            })
            $('#auth-error-launcher').modal('show');

            $(document).on('submit', '#login-metamask', function(event){
                event.preventDefault();

                $.ajax({
                    url: "{{ route('metamask.login') }}",
                    method: "POST",
                    data: {
                        "_token": "{{ csrf_token() }}",
                        "account": $('#exampleInputPassword1').val()
                    },
                    success:function(data){
                        if (data.success){
                            location.href = "{{ route('launcher.play') }}";
                        }
                        else{
                            alert("ERROR")
                        }
                    }
                })
            })
        })
    }, 1000);
</script>
