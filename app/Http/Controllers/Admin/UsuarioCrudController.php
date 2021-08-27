<?php

namespace App\Http\Controllers\Admin;

use App\Http\Requests\UsuarioRequest;
use Backpack\CRUD\app\Http\Controllers\CrudController;
use Backpack\CRUD\app\Library\CrudPanel\CrudPanelFacade as CRUD;

class UsuarioCrudController extends CrudController
{
    use \Backpack\CRUD\app\Http\Controllers\Operations\ListOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\CreateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\UpdateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\DeleteOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\ShowOperation;

    public function setup()
    {
        CRUD::setModel(\App\Models\Usuario::class);
        CRUD::setRoute(config('backpack.base.route_prefix') . '/usuario');
        CRUD::setEntityNameStrings('usuario', 'usuarios');
    }

    protected function setupListOperation()
    {
        $this->crud->addColumn([
            'name' => 'nombre',
            'label' => 'Nombre',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'metamask',
            'label' => 'Metamask',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'token_uid',
            'label' => 'UID',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'oro',
            'label' => 'Creditos',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'email',
            'label' => 'Email',
            'type' => 'email'
        ]);
        $this->crud->addColumn([
            'name' => 'ip_registro',
            'label' => 'IP Registro',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'ip_actual',
            'label' => 'IP actual',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'ultima_conexion',
            'label' => 'Ultima conexion',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'fecha_registro',
            'label' => 'Fecha registro',
            'type' => 'text'
        ]);
    }
    protected function setupCreateOperation()
    {
        CRUD::setValidation(UsuarioRequest::class);

        CRUD::setFromDb(); // fields
    }

    protected function setupUpdateOperation()
    {
        $this->setupCreateOperation();
    }
}
