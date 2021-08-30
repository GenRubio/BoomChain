<?php

namespace App\Http\Requests;

use App\Http\Requests\Request;
use Illuminate\Foundation\Http\FormRequest;

class UsuarioRequest extends FormRequest
{
    public function authorize()
    {
        return backpack_auth()->check();
    }
    public function rules()
    {
        return [
            'metamask' => 'required',
            'email' => 'required',
            'password' => 'required',
            'oro' => 'required',
            'admin' => 'required'
        ];
    }

    public function attributes()
    {
        return [
            //
        ];
    }
    public function messages()
    {
        return [
            //
        ];
    }
}
