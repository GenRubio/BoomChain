<script>
    setTimeout(function() {
        $(document).on("click", ".plus-ninjas", function() {
            removeOpenContainers();

            $(".contenedor-trajes-ninja").removeClass("none");
            $(".contenedor-trajes-ninja").addClass("block");
        });

        $(document).on("click", ".close-contenedor-trajes-ninja", function() {
            $(".contenedor-trajes-ninja").removeClass("block");
            $(".contenedor-trajes-ninja").addClass("none");
        });

        $(document).on('click', '.user-fichas-left-div', function() {
            removeOpenContainers();

            $(".user-fichas-div").removeClass("none");
            $(".user-fichas-div").addClass("block");
        });

        $(document).on('click', '.close-contenedor-user-fichas', function() {
            $(".user-fichas-div").removeClass("block");
            $(".user-fichas-div").addClass("none");
        });
      
    }, 1500);

    function removeOpenContainers() {
        removeContenedorTrajesNinja();
        removeContenedorUserFichas();

        function removeContenedorUserFichas() {
            $(".user-fichas-div").removeClass("block");
            $(".user-fichas-div").addClass("none");
        }

        function removeContenedorTrajesNinja() {
            $(".contenedor-trajes-ninja").removeClass("block");
            $(".contenedor-trajes-ninja").addClass("none");
        }
    }

    function entrarSala() {
        setTimeout(function() {

            addUserFichasLeftDiv();
            addPlusNinjas();
            removeSettingsButton();

        }, 460);

        function removeSettingsButton() {
            $(".settings-button").removeClass("block");
            $(".settings-button").addClass("none");
        }

        function addPlusNinjas() {
            if ("{{ $user->puntos_ninja }}" >= 400) {
                $(".plus-ninjas").removeClass("none");
                $(".plus-ninjas").addClass("block");
            }
        }

        function addUserFichasLeftDiv() {
            $(".user-fichas-left-div").removeClass("none");
            $(".user-fichas-left-div").addClass("block");
        }
    }

    function salirSala() {
        removeUserFichasLeftDiv();
        removePlusNinjas();

        function removePlusNinjas() {
            $(".plus-ninjas").removeClass("block");
            $(".plus-ninjas").addClass("none");
        }

        function removeUserFichasLeftDiv() {
            $(".user-fichas-left-div").removeClass("block");
            $(".user-fichas-left-div").addClass("none");
        }

        removeOpenContainers();
    }
</script>
