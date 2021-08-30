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
            $content = view('components.nav-bar.modal-login')->render();
        } else {
            $content = view('components.nav-bar.modal-register', [
                'metamask' => $request->account
            ])->render();
        }
        return response()->json([
            'content' => $content,
        ], Response::HTTP_CREATED);
    }

    public function register(Request $request){

        $metamask = $request->metamask;
        $password = $request->password;
        $password_confirm = $request->password_confirm;

        if ($password != $password_confirm){
            $success = false;
            $message = "Las contraseñas no coinciden";
        }
        else{
            $user = $this->getUser($metamask);
            if ($user){
                $success = false;
                $message = "Ha ocurrido un error";
            }
            else{

                $usuario = new Usuario();
                $usuario->nombre = $this->createUserName();
                $usuario->password = Hash::make($password_confirm);
                $usuario->avatar = 1;
                $usuario->colores = "B88A5CFF99000099CC0099CCE31709FFFFFF336666";
                $usuario->edad = 14;
                $usuario->ip_registro = "";
                $usuario->ip_actual = "";
                $usuario->fecha_registro = date("Y-m-d H:i:s");
                $usuario->token_uid = $this->getUniqueUID();
                $usuario->metamask = $request->metamask;
                $usuario->save();

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

    private function getUser($metamask){
        return Usuario::where('metamask', $metamask)->first();
    }

    private function createUserName()
    {
        $name = uniqid();
        while (Usuario::where('nombre', $name)->first() != null) {
            $name = uniqid();
        }
        return $name;
    }

    //Canal para recibir sockets
    private function getUniqueUID()
    {
        $uid = uniqid('token_uid-');
        while (Usuario::where('token_uid', $uid)->first() != null) {
            $uid = uniqid('token_uid-');
        }
        return $uid;
    }
}
