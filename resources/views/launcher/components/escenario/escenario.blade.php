<style>
    .plus-ninjas {
        color: white;
        cursor: pointer;
        position: absolute;
        margin-top: 113px;
        margin-left: 846px;
        font-weight: bold;
        font-size: 21px;
        font-family: cursive;
        transition: all 100ms ease;
    }

    .plus-ninjas:hover {
        color: rgb(233, 231, 231);
    }

    .user-fichas-left-div {
        height: 45px;
        width: 49px;
        background-color: white;
        border-radius: 12px;
        margin-top: 116px;
        margin-left: 9px;
        cursor: pointer;
        z-index: 2;
        position: absolute;
    }

    .contenedor-trajes-ninja {
        background-color: #ffffffd1;
        position: absolute;
        width: 220px;
        height: 100px;
        border-radius: 11px;
        margin-top: 169px;
        border: solid 1px #cccccc;
        margin-left: 581px;
    }

    .close-contenedor-trajes-ninja {
        font-weight: bold;
        font-size: 19px;
        margin-left: 203px;
        margin-top: -6px;
        cursor: pointer;
    }

    .trajes-ninja-img {
        height: auto;
        left: 50%;
        position: absolute;
        top: 50%;
        transform: translate(-50%, -50%);
        transition: 0.3s;
        max-width: none;
        width: 100%;
    }

    .user-ficha-img {
        height: auto;
        left: 50%;
        position: absolute;
        top: 50%;
        transform: translate(-50%, -50%);
        transition: 0.3s;
        max-width: none;
        width: 100%;
    }

    .user-ficha-div {
        position: relative;
        height: 111px;
        width: 83px;
        cursor: pointer;
        overflow: hidden;
    }


    .trajes-ninja-separator {
        margin-left: 5px !important;
        margin-bottom: 9px;
    }

    .trajes-ninja-div {
        position: relative;
        height: 35px;
        width: 35px;
        overflow: hidden;
        border-radius: 7px;
        margin-left: 7px;
        margin-top: -6px;
        border: 1px solid #dcd6d6;
    }

    .traje-div-enabled {
        background-color: white;
        cursor: pointer;
        transition: all 100ms ease;
    }

    .traje-div-enabled:hover {
        background-color: rgb(228, 227, 227);
        cursor: pointer;
    }

    .traje-div-disabled {
        background-color: #d2c1c1;
    }

    .user-fichas-div {
        position: absolute;
        min-height: 280px;
        width: 497px;
        background-color: #ffffffd1;
        margin-left: 69px;
        margin-top: 7px;
        border-radius: 10px;
        border: solid 1px #cccccc;
    }

    .close-contenedor-user-fichas {
        margin-left: 478px;
        font-size: 20px;
        font-weight: bold;
        margin-top: -4px;
        cursor: pointer;
    }

    .margin-left-5 {
        margin-left: 10px;
        margin-bottom: 10px;
    }

    .title-armario-fichas {
        font-weight: bold;
        margin-left: 10px;
        margin-top: -10px;
        font-size: 22px;
        width: 454px;
        padding: 5px;
        margin-bottom: 5px;
        border-radius: 10px;
        background-color: #ffffff9c;
    }

</style>

<div class="user-fichas-div none">
    <div class="close-contenedor-user-fichas">
        x
    </div>
    <div class="title-armario-fichas">
        Armario de fichas
    </div>
    <div class="d-flex">
        <div class="user-ficha-div margin-left-5" data-ficha="1002">
            <img class="user-ficha-img" src="{{ url('/images/fichas/fichaCeleste.png') }}" alt="Card image cap">
        </div>
        <div class="user-ficha-div margin-left-5" data-ficha="1004">
            <img class="user-ficha-img" src="{{ url('/images/fichas/fichaChiclet.png') }}" alt="Card image cap">
        </div>
        <div class="user-ficha-div margin-left-5" data-ficha="1008">
            <img class="user-ficha-img" src="{{ url('/images/fichas/fichaVerde.png') }}" alt="Card image cap">
        </div>
        <div class="user-ficha-div margin-left-5" data-ficha="1003">
            <img class="user-ficha-img" src="{{ url('/images/fichas/fichaMagenta.png') }}" alt="Card image cap">
        </div>
        <div class="user-ficha-div margin-left-5" data-ficha="1001">
            <img class="user-ficha-img" src="{{ url('/images/fichas/fichaMoradaClara.png') }}" alt="Card image cap">
        </div>
    </div>
    <div class="d-flex mb-2">
        <div class="user-ficha-div margin-left-5" data-ficha="1011">
            <img class="user-ficha-img" src="{{ url('/images/fichas/fichaMuscle.png') }}" alt="Card image cap">
        </div>
        <div class="user-ficha-div margin-left-5" data-ficha="1007">
            <img class="user-ficha-img" src="{{ url('/images/fichas/fichaNaranja.png') }}" alt="Card image cap">
        </div>
        <div class="user-ficha-div margin-left-5" data-ficha="1010">
            <img class="user-ficha-img" src="{{ url('/images/fichas/fichaPlatina.png') }}" alt="Card image cap">
        </div>
        <div class="user-ficha-div margin-left-5" data-ficha="1006">
            <img class="user-ficha-img" src="{{ url('/images/fichas/fichaRoja.png') }}" alt="Card image cap">
        </div>
        <div class="user-ficha-div margin-left-5" data-ficha="1009">
            <img class="user-ficha-img" src="{{ url('/images/fichas/fichaRoza.png') }}" alt="Card image cap">
        </div>
    </div>
</div>

<div class="user-fichas-left-div none"></div>

<div class="plus-ninjas none">
    +
</div>
<div class="contenedor-trajes-ninja none">
    <div class="close-contenedor-trajes-ninja">
        x
    </div>
    <div class="d-flex">
        <div class="trajes-ninja-div {{ $user->puntos_ninja >= 400 ? 'traje-div-enabled' : 'traje-div-disabled' }}"
            data-ninja="1">
            <img class="trajes-ninja-img" src="{{ url('/images/ninja-levels/game_12_1.png') }}" alt="Card image cap">
        </div>
        <div class="trajes-ninja-div trajes-ninja-separator {{ $user->puntos_ninja >= 410 ? 'traje-div-enabled' : 'traje-div-disabled' }}"
            data-ninja="2">
            <img class="trajes-ninja-img" src="{{ url('/images/ninja-levels/game_12_2.png') }}" alt="Card image cap">
        </div>
        <div class="trajes-ninja-div trajes-ninja-separator {{ $user->puntos_ninja >= 420 ? 'traje-div-enabled' : 'traje-div-disabled' }}"
            data-ninja="3">
            <img class="trajes-ninja-img" src="{{ url('/images/ninja-levels/game_12_3.png') }}" alt="Card image cap">
        </div>
        <div class="trajes-ninja-div trajes-ninja-separator {{ $user->puntos_ninja >= 450 ? 'traje-div-enabled' : 'traje-div-disabled' }}"
            data-ninja="4">
            <img class="trajes-ninja-img" src="{{ url('/images/ninja-levels/game_12_4.png') }}" alt="Card image cap">
        </div>
        <div class="trajes-ninja-div trajes-ninja-separator {{ $user->puntos_ninja >= 500 ? 'traje-div-enabled' : 'traje-div-disabled' }}"
            data-ninja="5">
            <img class="trajes-ninja-img" src="{{ url('/images/ninja-levels/game_12_5.png') }}" alt="Card image cap">
        </div>
    </div>
    <div class="d-flex">
        <div class="trajes-ninja-div {{ $user->puntos_ninja >= 550 ? 'traje-div-enabled' : 'traje-div-disabled' }}"
            data-ninja="6">
            <img class="trajes-ninja-img" src="{{ url('/images/ninja-levels/game_12_6.png') }}" alt="Card image cap">
        </div>
        <div class="trajes-ninja-div trajes-ninja-separator {{ $user->puntos_ninja >= 600 ? 'traje-div-enabled' : 'traje-div-disabled' }}"
            data-ninja="7">
            <img class="trajes-ninja-img" src="{{ url('/images/ninja-levels/game_12_7.png') }}" alt="Card image cap">
        </div>
        <div class="trajes-ninja-div trajes-ninja-separator {{ $user->puntos_ninja >= 700 ? 'traje-div-enabled' : 'traje-div-disabled' }}"
            data-ninja="8">
            <img class="trajes-ninja-img" src="{{ url('/images/ninja-levels/game_12_8.png') }}" alt="Card image cap">
        </div>
        <div class="trajes-ninja-div trajes-ninja-separator {{ $user->puntos_ninja >= 800 ? 'traje-div-enabled' : 'traje-div-disabled' }}"
            data-ninja="9">
            <img class="trajes-ninja-img" src="{{ url('/images/ninja-levels/game_12_9.png') }}" alt="Card image cap">
        </div>
        <div class="trajes-ninja-div trajes-ninja-separator {{ $user->puntos_ninja >= 1000 ? 'traje-div-enabled' : 'traje-div-disabled' }}"
            data-ninja="10">
            <img class="trajes-ninja-img" src="{{ url('/images/ninja-levels/game_12_10.png') }}"
                alt="Card image cap">
        </div>
    </div>
</div>
