<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UsuarioUpdateRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     *
     * @return bool
     */
    public function authorize()
    {
        return backpack_auth()->check();
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array
     */
    public function rules()
    {
        return [
            'metamask' => 'required',
            'email' => 'required',
            'password' => 'required',
            'oro' => 'required',
            'admin' => 'required',
            'nombre' => 'required',
            'ip_registro' => 'required',
            'ip_actual' => 'required',
            'ultima_conexion' => 'required',
            'fecha_registro' => 'required',
        ];
    }
}
