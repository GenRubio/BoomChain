<style>
    .form-control {
        border: none !important;
        font-size: 16px !important;
        height: calc(1.6em + 0.75rem + 10px) !important;
        background-color: #f5f5f5;
    }

    .form-control-register {
        border: none !important;
        font-size: 14px !important;
        height: calc(1.6em + 0.75rem + 8px) !important;
        background-color: #f5f5f5;
    }

    .form-control:hover {
        background-color: #f5f5f5;
    }

    .btn-primary {
        height: 47px !important;
        font-size: 18px;
    }

    .separator-left {
        width: 187px;
        margin-top: -5px;
    }

    .separator-right {
        width: 187px;
        margin-top: -5px;
    }

    .o-separator {
        margin-left: 5px;
        margin-right: 5px;
    }

    .button-google {
        width: 100%;
        height: 50px;
        background-color: white;
        border-radius: 5px;
        border: 1px solid #e2e2e2;
        cursor: pointer;
        transition: all 230ms ease;
    }

    .button-google:hover {
        background-color: rgb(240, 239, 239);
    }

    .button-google-text {
        font-family: inherit;
        font-size: 18px;
        margin-left: 119px;
        margin-top: 10px;
    }

    .google-image-div {
        position: absolute;
        height: 41px;
        width: 40px;
        overflow: hidden;
        margin-left: 71px;
        margin-top: 3px;
    }

    .google-image-img {
        height: auto;
        left: 50%;
        position: absolute;
        top: 50%;
        transform: translate(-50%, -50%);
        transition: 0.3s;
        max-width: none;
        width: 100%;
    }

</style>
<br><br><br><br><br><br>
<h1>Iniciar sesión</h1>
<br>
<form id="login-form">
    @csrf
    <div class="form-group">
        <label for="exampleInputEmail1">MetaMask Account ID</label>
        <input name="account" type="password" class="form-control" id="metamask-input" aria-describedby="emailHelp"
            placeholder="Introduce tu id de MetaMask" required>
    </div>
    <div class="d-flex justify-content-between">
        <div>
            <div class="custom-control custom-switch">
                <input name="auto_login" type="checkbox" class="custom-control-input" id="customSwitch1">
                <label class="custom-control-label" for="customSwitch1">Auto login</label>
            </div>
        </div>
    </div>
    <br><br>
    <button type="submit" class="btn btn-primary btn-block" id="play-button">Iniciar sesíon</button>
</form>

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
                        "account": $('#metamask-input').val()
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
