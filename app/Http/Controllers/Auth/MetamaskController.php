<?php

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use App\Models\Usuario;
use Illuminate\Http\Request;
use Illuminate\Http\Response;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Hash;

class MetamaskController extends Controller
{
    public function index(Request $request)
    {
        $user = $this->getUser($request->account);

        if ($user) {
            $content = view('components.nav-bar.modal-login', [
                'metamask' => $request->account
            ])->render();
        } else {
            $content = view('components.nav-bar.modal-register', [
                'metamask' => $request->account
            ])->render();
        }
        return response()->json([
            'content' => $content,
        ], Response::HTTP_CREATED);
    }

    public function logOut()
    {
        Auth::logout();
    }

    public function login(Request $request)
    {
        $metamask = $request->account;

        $user = $this->getUser($metamask);

        if ($user) {
            Auth::login($user);
            $message = "";
            $success = true;
        } else {
            $message = "Este usuario no existe";
            $success = false;
        }

        return response()->json([
            'success' => $success,
            'message' => $message
        ], Response::HTTP_CREATED);
    }

    public function loginWeb(Request $request){
        $metamask = $request->account;
        $password = $request->password;

        

        $credentials = ['metamask' => $metamask, 'password' => $password];
        $message = "";

        if (Auth::attempt($credentials, true)) {
            $success = true;
        } else {
            $success = false;
            $message = "Metamask o contraseña incorrectos";
        }
        return response()->json([
            'success' => $success,
            'message' => $message,
        ], Response::HTTP_CREATED);
    }

    public function register(Request $request)
    {

        $metamask = $request->metamask;
        $password = $request->password;
        $password_confirm = $request->password_confirm;

        if ($password != $password_confirm) {
            $success = false;
            $message = "Las contraseñas no coinciden";
        } else {
            $user = $this->getUser($metamask);
            if ($user) {
                $success = false;
                $message = "Ha ocurrido un error";
            } else {

                $usuario = $this->createUser($request);
                Auth::login($usuario);

                $success = true;
                $message = "";
            }
        }
        return response()->json([
            'success' => $success,
            'message' => $message
        ], Response::HTTP_CREATED);
    }

    private function createUser(Request $request)
    {

        $usuario = new Usuario();
        $usuario->metamask = $request->metamask;
        $usuario->password = Hash::make($request->password_confirm);
        $usuario->nombre = null;
        $usuario->avatar = 1;
        $usuario->colores = "B88A5CFF99000099CC0099CCE31709FFFFFF336666";
        $usuario->ip_registro = null;
        $usuario->ip_actual = null;
        $usuario->fecha_registro = null;
        $usuario->token_uid = null;
        $usuario->save();

        return $usuario;
    }
    private function getUser($metamask)
    {
        return Usuario::where('metamask', $metamask)->first();
    }
}
