<!-- This file is used to store sidebar items, starting with Backpack\Base 0.9.0 -->
<li class="nav-item"><a class="nav-link" href="{{ backpack_url('dashboard') }}"><i
            class="la la-home nav-icon"></i> {{ trans('backpack::base.dashboard') }}</a></li>
<li class='nav-item'><a class='nav-link' href='{{ backpack_url('usuario') }}'><i class='nav-icon la la-question'></i>
        Usuarios</a></li>
<li class='nav-item'><a class='nav-link' href='{{ backpack_url('escenarios-publico') }}'><i
            class='nav-icon la la-question'></i> Escenarios</a></li>

<li class="nav-item nav-dropdown">
    <a class="nav-link nav-dropdown-toggle" href="#"><i class="nav-icon la la-question"></i>Catalago</a>
    <ul class="nav-dropdown-items">
        <li class='nav-item'>
            <a class='nav-link' href='{{ backpack_url('catalago-objeto') }}'>
                <i class='nav-icon la la-question'></i> Objetos
            </a>
        </li>
        <li class='nav-item'>
            <a class='nav-link' href='{{ backpack_url('catalago-planta') }}'>
                <i class='nav-icon la la-question'></i> Plantas
            </a>
        </li>
    </ul>
</li>
