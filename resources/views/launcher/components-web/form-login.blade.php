
<div>
    <div class="d-flex justify-content-between">
        <div>
        </div>
        <div style="color: grey;">
            ¿No tienes cuenta? <a href="{{ route('home') }}" target="_blank" 
                style="text-decoration: none;">Regístrate</a>
        </div>
    </div>
</div>
<br><br><br><br>
<h1>Iniciar sesión</h1>
<br>
<form id="login-form">
    @csrf
    <div class="form-group">
        <label for="exampleInputEmail1">MetaMask Account ID</label>
        <input type="text" class="form-control" id="metamask-input" aria-describedby="emailHelp"
            placeholder="Introduce tu id de MetaMask" required>
    </div>
    <div class="form-group">
        <label for="exampleInputEmail1">Contraseña</label>
        <input type="password" class="form-control" id="password-input" aria-describedby="emailHelp"
            placeholder="Introduce tu contarseña" required>
    </div>
    <br><br>
    <button type="submit" class="btn btn-primary btn-block" id="play-button">Iniciar sesíon</button>
</form>
<div style="margin-top: 117px;">
    Game version {{ Config::get('app.game_v'); }}
</div>
<script>
    setTimeout(function() {
        $(document).ready(function() {

            $(document).on('submit', '#login-form', function(event) {
                event.preventDefault();
                $('#play-button').attr('disabled', true);

                $.ajax({
                    url: "{{ route('metamask.login') }}",
                    method: "POST",
                    data: {
                        "_token": "{{ csrf_token() }}",
                        "account": $('#metamask-input').val(),
                        "password": $("#password-input").val()
                    },
                    success: function(data) {
                        if (data.success) {
                            location.href = "{{ route('launcher.play') }}";
                        } else {
                            toastr.options.closeButton = true;
                            toastr.error(data.message);

                            $('#play-button').attr('disabled', false);
                            $('#login-form')[0].reset();
                        }
                    }
                })
            })
        })
    }, 1000);
</script>
