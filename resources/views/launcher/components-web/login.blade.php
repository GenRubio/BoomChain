
<div id="login-page">
    <div class="d-flex">
        <div class="container-height-left position">
            <div class="pl-4 pr-4 pt-3 container-login-page">
                @include('launcher.components-web.form-login')
            </div>
        </div>
        <div class="logo-image-div">
            <div>
                <img class="logo-image-img" src="{{ url('/images/logo_2.png') }}" alt="Card image cap">
            </div>
        </div>
        <div class="comando-reload">
            <div class="p-2">
                Nuevo comando disponible en el launcher (F5)<br>
                Reinicia el cliente sin tener que cerrar totalmente el juego.
            </div>
        </div>
        <div class="container-height-right">
            <div class="internal-right-container" style="background-image: url({{ url('images/cover3.png') }})">
            </div>
        </div>
    </div>
</div>
<script>

</script>
