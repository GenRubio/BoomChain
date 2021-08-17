<style>
    .container-height-left {
        height: 658px;
        width: 440px;
        background-color: white;
    }

    .container-height-right {
        height: 658px;
        width: 601px;
        margin-left: 413px;
    }

    .internal-right-container {
        height: 100%;
        width: 100%;
        background-repeat: repeat;
    }

    #login-page {
        cursor: context-menu;
        -webkit-touch-callout: none;
        -webkit-user-select: none;
        -khtml-user-select: none;
        -moz-user-select: none;
        -ms-user-select: none;
        user-select: none;
    }

    .position {
        position: absolute;
    }

    .comando-reload {
        z-index: 3;
        position: absolute;
        margin-left: 491px;
        margin-top: 256px;
        border-radius: 6px;
        height: 61px;
        width: 473px;
        border: 1px solid #9e9b9b;
        background-color: white;
    }

    .logo-image-div {
        z-index: 3;
        margin-left: 559px;
        position: absolute;
        height: 255px;
        width: 354px;
        overflow: hidden;
    }

    .logo-image-img {
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
