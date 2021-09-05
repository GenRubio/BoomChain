
{{-- ----------------------------------------------Contenedores------------------------------------------------ --}}

@include('launcher.components.escenario.escenario')
@include('launcher.components.flowerPower.flower-power')

{{-- ----------------------------------------------Logica------------------------------------------------------ --}}
<script src="https://cdn.socket.io/4.1.2/socket.io.min.js"
integrity="sha384-toS6mmwu70G0fw54EGlWWeA4z3dyJ+dlXBtSURSKN4vyRFOcxd3Bzjj/AoOwY+Rg" crossorigin="anonymous">
</script>

@include('launcher.components.escenario.escenario-js')
@include('launcher.components.flowerPower.flower-power-js')

<script type="text/javascript">
    var client = {
        token_uid: "{{ $user->token_uid }}",
    }
    var socketUser = null;

    setTimeout(function() {
        const socket = io('http://127.0.0.1:3000');
        socketUser = socket;

        socket.on('entrarSala-' + client.token_uid, (message) => {
            entrarSala();
        });

        socket.on('salirSala-' + client.token_uid, (message) => {
            salirSala();
        });

        socket.on('entrarFlowerPower-' + client.token_uid, () => {
            cargarFlowerPower();
        })

        $(document).on('click', '.user-ficha-div', function() {
            let ficha = $(this).data('ficha');
            let data = {
                'token_uid': "{{ $user->token_uid }}",
                'ficha': ficha,
            };
            socket.emit("change-ficha-user", data);
            salirSala();
        });

        $(document).on('click', ".traje-div-enabled", function() {
            let traje = $(this).data('ninja');
            let data = {
                'token_uid': "{{ $user->token_uid }}",
                'cintaNinja': traje,
            };
            socket.emit("change-ninja", data);
        })

    }, 1500);

    function isReady() {
        console.log("Login is ready")
    }
</script>
