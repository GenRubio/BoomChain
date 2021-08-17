@extends('layouts.app')
@section('title')
    <title>BoomBang Game</title>
@endsection
@section('personal-style')
    <style>
        .container-game {
            width: 1013px;
            height: 658px;
            position: absolute;
            z-index: -1;
        }

        #flash_content_play {
            cursor: context-menu;
            -webkit-touch-callout: none;
            -webkit-user-select: none;
            -khtml-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

    </style>
@endsection

@section('content')
    <div id="flash_content_play">
        @auth
            @include('launcher.components.flash-content-play', ['user' => auth()->user()])
        @else
            @include('launcher.components.modal-auth-error')
        @endauth
    </div>
@endsection

@section('personal-script')
    <script>
       
    </script>

@endsection
