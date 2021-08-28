<?php

namespace App\Http\Controllers\Admin;

use App\Http\Requests\UsuarioRequest;
use Backpack\CRUD\app\Http\Controllers\CrudController;
use Backpack\CRUD\app\Library\CrudPanel\CrudPanelFacade as CRUD;
use Illuminate\Support\Facades\Route;

class PersonajeCrudController extends CrudController
{
    use \Backpack\CRUD\app\Http\Controllers\Operations\ListOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\CreateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\UpdateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\DeleteOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\ShowOperation;

    protected $user_id;

    public function setup()
    {
        CRUD::setModel(\App\Models\Usuario::class);
        $this->setRoute();
        $this->breadCrumbs();
        $this->filterList();
        CRUD::setEntityNameStrings('personaje', 'personaje');
    }

    private function setRoute()
    {
        $this->user_id = Route::current()->parameter('user_id');

        $this->crud->setRoute(
            "admin/usuario/"
                . $this->user_id . "/personaje"
        );
    }
    private function breadCrumbs()
    {
        $this->data['breadcrumbs'] = [
            trans('backpack::crud.admin') => backpack_url('dashboard'),

            'Usuarios' => backpack_url('usuario'),
            'Personaje' => backpack_url("usuario/"
                . $this->user_id
                . "/personaje"),
            trans('backpack::crud.list') => false,
        ];
    }

    private function filterList()
    {
        $this->crud->addClause('where', 'id', $this->user_id);
    }

    protected function setupListOperation()
    {
        $this->crud->removeButton('create');
        $this->crud->removeButton('delete');

        $this->crud->addColumn([
            'name' => 'nombre',
            'label' => 'Nombre',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'bocadillo',
            'label' => 'Descripcion',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'avatar',
            'label' => 'Avatar',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'colores',
            'label' => 'Colores',
            'type' => 'text'
        ]);
        $this->crud->addColumn([
            'name' => 'edad',
            'label' => 'Color ficha',
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
