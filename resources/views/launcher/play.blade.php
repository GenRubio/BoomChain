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
            @include('components.play', ['user' => auth()->user(), 'google' => true])
        @else
            @include('components.login-page')
        @endauth
    </div>
    <script>
        setTimeout(function() {
            $(document).ready(function() {

                $("#nombre-input").focus();
                $('#login-form').submit(function(event) {
                    $('#play-button').attr('disabled', true);
                    $('#play-button').text("Cargando...");

                    event.preventDefault();

                    $.ajax({
                        url: "{{ route('login') }}",
                        method: "POST",
                        data: $(this).serialize(),
                        success: function(data) {
                            if (data.success) {
                                $('#flash_content_play').html(data.content);
                            } else {
                                toastr.options.closeButton = true;
                                toastr.error(data.message);

                                $('#play-button').attr('disabled', false);
                                $('#play-button').text("Iniciar sesíon");
                                $('#login-form')[0].reset();
                            }
                        }
                    })
                });

                $('#register-form').submit(function(event) {
                    $(".submit-register-button").attr('disabled', true);
                    $('.submit-register-button').text("Cargando...");

                    event.preventDefault();

                    $.ajax({
                        url: "{{ route('register') }}",
                        method: "POST",
                        data: $(this).serialize(),
                        success: function(data) {
                            if (data.success) {
                                $('#registerModal').modal('hide');
                                toastr.options.closeButton = true;
                                toastr.success(data.message);
                                setTimeout(function() {
                                    $('#flash_content_play').html(data.content);
                                }, 200);
                            } else {
                                toastr.options.closeButton = true;
                                toastr.error(data.message);
                            }
                            $('.submit-register-button').attr('disabled', false);
                            $('.submit-register-button').text("Regístrarse");
                        }
                    })
                });

                $('#recover-password-form').submit(function(event) {
                    $(".submit-reocever-1-button").attr('disabled', true);
                    $('.submit-reocever-1-button').text("Cargando...");

                    event.preventDefault();

                    $.ajax({
                        url: "{{ route('recover.password.1') }}",
                        method: "POST",
                        data: $(this).serialize(),
                        success: function(data) {
                            if (data.success) {
                                $('.modal-recover-password').html(data.content);

                                toastr.options.closeButton = true;
                                toastr.success(data.message)
                            } else {
                                toastr.options.closeButton = true;
                                toastr.error(data.message);

                                $('.submit-reocever-1-button').attr('disabled', false);
                                $('.submit-reocever-1-button').text(
                                    "Recuperar contraseña");
                            }
                        }
                    })
                });

                $(document).on('submit', '#validate-recover-key-form', function(event) {
                    $(".submit-reocever-2-button").attr('disabled', true);
                    $('.submit-reocever-2-button').text("Cargando...");

                    event.preventDefault();

                    $.ajax({
                        url: "{{ route('recover.password.2') }}",
                        method: "POST",
                        data: $(this).serialize(),
                        success: function(data) {
                            if (data.success) {
                                $('.modal-recover-password').html(data.content);

                            } else {
                                toastr.options.closeButton = true;
                                toastr.error(data.message);

                                $('.submit-reocever-2-button').attr('disabled', false);
                                $('.submit-reocever-2-button').text(
                                    "Validar");

                                $('#validate-recover-key-form')[0].reset();
                            }
                        }
                    })
                });

                $(document).on('submit', '#change-password-form', function(event) {
                    $(".change-reocever-1-button").attr('disabled', true);
                    $('.change-reocever-1-button').text("Cargando...");

                    event.preventDefault();

                    $.ajax({
                        url: "{{ route('recover.password.3') }}",
                        method: "POST",
                        data: $(this).serialize(),
                        success: function(data) {
                            if (data.success) {
                                toastr.options.closeButton = true;
                                toastr.success(data.message);
                                $('#recoverPasswordModal').modal('hide');
                            } else {
                                toastr.options.closeButton = true;
                                toastr.error(data.message);
                                $('#change-password-form')[0].reset();
                            }
                            $('.change-reocever-1-button').attr('disabled', false);
                            $('.change-reocever-1-button').text(
                                "Cambiar contraseña");
                        }
                    })
                });
            })
        }, 1500)
    </script>

@endsection
